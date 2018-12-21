using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/ticket/reaction")]
    public class TicketReactionController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public TicketReactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        // GET api/faq/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ticketReactions = _context.TicketReactions.Include(x => x.Ticket).Include(x => x.From).Where(x => x.TicketId.Equals(id));

            return Json(ticketReactions);
        }

        // POST api/ticket/reaction
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]TicketReactionViewModel value)
        {
            if (!TryValidateModel(value)) return StatusCode(500, ModelState);

            var reaction = new TicketReaction();
            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.FromId);

            reaction.FromId = discordUser.Id;
            reaction.TicketId = value.TicketId;

            reaction.DiscordMessage = new DiscordMessage();

            reaction.DiscordMessage.ChannelId = value.DiscordMessage.ChannelId;
            reaction.DiscordMessage.IsDm = value.DiscordMessage.IsDm;
            reaction.DiscordMessage.MessageId = value.DiscordMessage.MessageId;
            reaction.DiscordMessage.IsEmbed = value.DiscordMessage.IsEmbed;
            reaction.DiscordMessage.Message = value.DiscordMessage.Message;
            reaction.DiscordMessage.Timestamp = value.DiscordMessage.Timestamp;
            reaction.DiscordMessage.GuildId = value.DiscordMessage.GuildId;

            var user = new DiscordUser();

            if (!string.IsNullOrWhiteSpace(value.FromId))
            {
                user = _context.DiscordUsers.SingleOrDefault(x => x.DiscordId == value.FromId);

                if (user == null)
                {
                    user = _context.DiscordUsers.FirstOrDefault();
                    
                    if (user != null)
                    {
                        return BadRequest($"Please contact mick about this ID: {user.Id}  & Discord ID: {user.DiscordId} - ticket reaction");
                    }

                    user = AddUser(value);
                }
            }
            else
            {
                user = AddUser(value);
            }

            reaction.FromId = user.Id;
            reaction.From = null;

            _context.TicketReactions.Add(reaction);
            _context.SaveChanges();

            return Created(Url.Action("Get", new { id = reaction.Id }), reaction);
        }

        private DiscordUser AddUser(TicketReactionViewModel value)
        {
            var user = new DiscordUser()
            {
                DiscordId = value.FromId,
                Username = value.Username
            };

            _context.Add(user);

            _context.SaveChanges();

            return user;

        }

        // PUT api/faq/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody]FrequentlyAskedQuestion value)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            faq.Answer = value.Answer;
            faq.Description = value.Description;
            faq.Question = value.Question;

            _context.SaveChanges();

            return Ok(faq);
        }

        // DELETE api/faq/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            _context.FrequentlyAskedQuestions.Remove(faq);
            _context.SaveChanges();

            return Delete(id);

        }
    }
}

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
            var ticketReactions = _context.TicketReactions.Include(x=>x.Ticket).Include(x=>x.From).Where(x => x.TicketId.Equals(id));
            
            return Json(ticketReactions);
        }

        // POST api/ticket/reaction
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]TicketReactionViewModel value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            var reaction = new TicketReaction();
            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.FromId);

            reaction.FromId = discordUser.Id;
            reaction.Message = value.Message;
            reaction.TicketId = value.TicketId;
            reaction.MessageId = value.MessageId;

            _context.TicketReactions.Add(reaction);
            _context.SaveChanges();

            //if (value.ResourceLink != null)
            //{
            //    var resourceLink = _context.ResourceLinks.FirstOrDefault(x =>
            //        x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link));

            //    if (resourceLink == null)
            //    {
            //        _context.ResourceLinks.Add(new ResourceLink()
            //        {
            //            DisplayName = value.ResourceLink.DisplayName,
            //            Link = value.ResourceLink.DisplayName
            //        });
            //        _context.SaveChanges();

            //        resourceLink = _context.ResourceLinks.First(x =>
            //            x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link)); ;
            //    }

            //    value.ResourceLinkId = resourceLink.Id;
            //}

            return Created(Url.Action("Get", new { id = reaction.Id }), reaction);
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

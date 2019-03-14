using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/ticket/reaction")]
    public class TicketReactionController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public TicketReactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
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
        public async Task<IActionResult> Post([FromBody]TicketReactionViewModel value)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add reactions to our tickets huh? NOOOOPE!");
            }

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

            reaction.From = user;

            //_hubContext.Clients.Group(RoleNames.HappyToHelp).SendAsync("TicketReaction", reaction);
            //_hubContext.Clients.Group($"Ticket${reaction.TicketId}").SendAsync("TicketReaction", reaction);
            _hubContext.Clients.All.SendAsync("TicketReaction", reaction);

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

        // DELETE api/faq/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to delete ticket reactions huh? NOOOOPE!");
            }

            var ticketReaction = _context.TicketReactions.FirstOrDefault(x => x.Id == id);

            if (ticketReaction == null) return NotFound();

            _context.TicketReactions.Remove(ticketReaction);
            _context.SaveChanges();


            _hubContext.Clients.All.SendAsync("DeleteTicketReaction", ticketReaction);

            return Ok(id);

        }
    }
}

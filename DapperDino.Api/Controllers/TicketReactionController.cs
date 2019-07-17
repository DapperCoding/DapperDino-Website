using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models.Discord;
using DapperDino.Core;
using DapperDino.Core.Discord;
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
        private readonly DiscordEmbedHelper _discordEmbedHelper;

        #endregion

        #region Constructor(s)

        public TicketReactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<DiscordBotHub> hubContext, DiscordEmbedHelper discordEmbedHelper)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
            _discordEmbedHelper = discordEmbedHelper;
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

            if (string.IsNullOrWhiteSpace(value.DiscordMessage.Message))
            {
                value.DiscordMessage.Message = "EMPTY";
            }

            if (!TryValidateModel(value))
            {
                return StatusCode(500, ModelState);
            }

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
            try
            {

                using (Bot bot = new Bot())
                {
                    bot.RunAsync();

                    var client = bot.GetClient();

                    var guild = await client.GetGuildAsync(ulong.Parse(value.DiscordMessage.GuildId));
                    var channel = guild.GetChannel(ulong.Parse(value.DiscordMessage.ChannelId));
                    var message = await channel.GetMessageAsync(ulong.Parse(value.DiscordMessage.MessageId));

                    reaction.DiscordMessage = UpdateMessage(reaction.DiscordMessage, message);

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

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

            reaction = await _context.TicketReactions
                .Include(x => x.From)
                .Include(x => x.DiscordMessage)

                .Include(x => x.DiscordMessage)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Attachments)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Color)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Footer)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Image)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Thumbnail)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Video)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Provider)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Author)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id == reaction.Id);

            //_hubContext.Clients.Group(RoleNames.HappyToHelp).SendAsync("TicketReaction", reaction);
            //_hubContext.Clients.Group($"Ticket${reaction.TicketId}").SendAsync("TicketReaction", reaction);
            await _hubContext.Clients.All.SendAsync("TicketReactionTest", true);
            await _hubContext.Clients.All.SendAsync("TicketReaction", reaction);

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


        private DiscordMessage UpdateMessage(DiscordMessage dbMessage, DSharpPlus.Entities.DiscordMessage discordMessage)
        {

            // Add embeds from bot
            if (discordMessage.Embeds != null && discordMessage.Embeds.Any())
            {
                if (dbMessage.Embeds == null) dbMessage.Embeds = new List<DAL.Models.DiscordEmbed>();

                foreach (var embed in discordMessage.Embeds)
                {
                    dbMessage.Embeds.Add(_discordEmbedHelper.ConvertEmbed(embed));
                }
            }

            // Add attachments from bot
            if (discordMessage.Attachments != null && discordMessage.Attachments.Any())
            {
                dbMessage.Attachments = _discordEmbedHelper.ConvertAttachments(discordMessage.Attachments);
            }

            return dbMessage;
        }
    }
}

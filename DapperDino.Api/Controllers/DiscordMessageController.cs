using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models.Discord;
using DapperDino.Core;
using DapperDino.Core.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class DiscordMessageController : BaseController
    {

        #region Fields

        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DiscordEmbedHelper _discordEmbedHelper;

        #endregion

        #region Constructor(s)
        public DiscordMessageController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, DiscordEmbedHelper discordEmbedHelper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _discordEmbedHelper = discordEmbedHelper;
        }

        #endregion

        [HttpGet("")]
        public IActionResult Index()
        {
            // Create variable called messages
            var messages =

                // Access database
                _dbContext

                // Table "DiscordMessages" 
                .DiscordMessages

                // Order by id descending
                .OrderByDescending(x => x.Id)

                // Take most recent 200 messages
                .Take(200)

                // ToList() to execute query
                .ToList();


            // Return messages as JSON
            return Json(messages);
        }

        [HttpGet("{id}")]
        public IActionResult GetSingle(int id)
        {
            // Create variable called message
            var message =

                // Access database
                _dbContext

                // Table "DiscordMessages" 
                .DiscordMessages

                // Get single message with Id is equal to parameter id
                .SingleOrDefault(x => x.Id == id);


            // Return messages as JSON
            return Json(message);
        }

        [HttpPost("{id:int?}")]
        [Authorize]
        public async Task<IActionResult> CreateOrEdit(int? id, [FromBody]DiscordMessageModel message)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to create or edit a discordmessage huh? NOOOOPE!");
            }

            if (!ModelState.IsValid) return BadRequest(ModelState);


            // Create new DiscordMessage object
            var discordMessage = new DiscordMessage();

            // Check if the id parameter is filled with an int
            if (id.HasValue)
            {

                // Get single discord message
                var row = _dbContext.DiscordMessages.SingleOrDefault(x => x.Id == id.Value);


                // Check if discord message is not found
                if (row == null)
                {

                    // Return HTTP 404 Not Found, content found in string below, with string interpolated the id parameter
                    return NotFound($"Couldn't find DiscordMessage with id == {id.Value}");
                }


                // Set new discordMessage to the current DiscordMessage we found(will be edited later on)   
                discordMessage = row;
            }
            else
            {

                // Add new DiscordMessage to the table
                _dbContext.DiscordMessages.Add(discordMessage);
            }

            using (Bot bot = new Bot())
            {
                bot.RunAsync();

                var client = bot.GetClient();

                var guild = await client.GetGuildAsync(ulong.Parse(message.GuildId));
                var channel = guild.GetChannel(ulong.Parse(message.ChannelId));
                var newMessage = await channel.GetMessageAsync(ulong.Parse(message.MessageId));

                if (newMessage.Author == null)
                {
                    ulong userId;
                    if (ulong.TryParse(message.UserId, out userId))
                    {
                        await guild.GetMemberAsync(userId);
                    }
                }

                var user = _dbContext.DiscordUsers.SingleOrDefault(x => x.DiscordId == newMessage.Author.Id.ToString());

                if (user != null)
                {
                    discordMessage.DiscordUserId = user.Id;
                }

                discordMessage = UpdateMessage(discordMessage, newMessage);
            }


            // Check if IsDm is false and GuildId or ChannelId isn't filled
            if (!message.IsDm && (string.IsNullOrWhiteSpace(message.GuildId) || string.IsNullOrWhiteSpace(message.ChannelId)))
            {

                // Return BadRequest because both GuildId and ChannelId are required when IsDm is true
                return BadRequest("If IsDm is set to false, GuildId and ChannelId are required");

            }

            // Check if IsDm is true and UserId isn't filled
            else if (message.IsDm && string.IsNullOrWhiteSpace(message.UserId))
            {

                // Return BadRequest because UserId is required
                return BadRequest("If IsDm is set to true, UserId is required");
            }

            // Set value of DiscordMessage 
            discordMessage.GuildId = message.GuildId;
            discordMessage.IsEmbed = message.IsEmbed;
            discordMessage.IsDm = message.IsDm;
            discordMessage.Message = message.Message;
            discordMessage.MessageId = message.MessageId;
            discordMessage.Timestamp = message.Timestamp;

            _dbContext.SaveChanges();

            return Json(discordMessage);
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
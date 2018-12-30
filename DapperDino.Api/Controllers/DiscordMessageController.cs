using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class DiscordMessageController : BaseController
    {

        #region Fields

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructor(s)
        public DiscordMessageController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public IActionResult CreateOrEdit(int? id, [FromBody]DiscordMessageModel message)
        {
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


            // Check if IsDm is false and GuildId or ChannelId isn't filled
            if(!message.IsDm && (string.IsNullOrWhiteSpace(message.GuildId) || string.IsNullOrWhiteSpace(message.ChannelId)))
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

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class FaqController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region Constructor(s)

        public FaqController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion

        // GET api/faq
        [HttpGet]
        public IEnumerable<FrequentlyAskedQuestion> Get()
        {
            return _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).ToArray();
        }

        // GET api/faq/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var faq = _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).FirstOrDefault(x => x.Id == id);

            if (faq == null)
            {
                return NotFound();
            }

            return Json(faq);
        }

        // POST api/faq
        [HttpPost("")]
        //[Authorize]
        public async Task<IActionResult> Post([FromBody]FrequentlyAskedQuestion value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add items to our faq huh? NOOOOPE!");
            }

            if (!TryValidateModel(value)) return StatusCode(500);

            _context.FrequentlyAskedQuestions.Add(value);
            _context.SaveChanges();

            if (value.DiscordMessage != null && value.DiscordMessage.DiscordUser != null)
            {
                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.DiscordId == value.DiscordMessage.DiscordUser.DiscordId);

                if (discordUser == null)
                {
                    // TODO: use client to get user information
                    //_context.DiscordUsers.Add()
                }

                
                value.DiscordMessage.DiscordUserId = discordUser.Id;
            }
            

            if (value.ResourceLink != null)
            {
                var resourceLink = _context.ResourceLinks.FirstOrDefault(x =>
                    x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link));

                if (resourceLink == null)
                {
                    _context.ResourceLinks.Add(new ResourceLink()
                    {
                        DisplayName = value.ResourceLink.DisplayName,
                        Link = value.ResourceLink.DisplayName
                    });
                    _context.SaveChanges();

                    resourceLink = _context.ResourceLinks.First(x =>
                        x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link));
                }

                value.ResourceLinkId = resourceLink.Id;
                _context.SaveChanges();
            }

            return Created(Url.Action("Get", new { id = value.Id }), value);
        }

        // PUT api/faq/5
        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [FromBody]FrequentlyAskedQuestion value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to change items from our faq huh? NOOOOPE!");
            }

            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            faq.Answer = value.Answer;
            faq.Description = value.Description;
            faq.Question = value.Question;

            _context.SaveChanges();

            return Ok(faq);
        }

        // POST api/faq/5
        [HttpPost("/api/faq/addmessageid")]
        [Authorize]
        public async Task<IActionResult> AddFaqMessage([FromBody]FaqMessageIdViewModel value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to change items from our faq huh? NOOOOPE!");
            }

            var faq = _context.FrequentlyAskedQuestions.SingleOrDefault(x => x.Id == value.Id);
            
            if (faq == null) return NotFound($"Faq with id ${value.Id} couldn't be found");

            var discordMessage = new DiscordMessage();

            if (faq.DiscordMessageId.HasValue)
            {
                discordMessage = _context.DiscordMessages.SingleOrDefault(x => x.Id == faq.DiscordMessageId.Value);
            } 
            else
            {
                _context.DiscordMessages.Add(discordMessage);
            }

            discordMessage.ChannelId = value.Message.ChannelId;
            discordMessage.GuildId = value.Message.GuildId;
            discordMessage.IsDm = value.Message.IsDm;
            discordMessage.IsEmbed = value.Message.IsEmbed;
            discordMessage.Message = string.IsNullOrWhiteSpace(value.Message.Message) ? "" : value.Message.Message;
            discordMessage.MessageId = value.Message.MessageId;
            discordMessage.Timestamp = value.Message.Timestamp;

            _context.SaveChanges();

            faq.DiscordMessageId = discordMessage.Id;

            _context.SaveChanges();

            faq.DiscordMessage = discordMessage;

            return Ok(faq);
        }

        // DELETE api/faq/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to change items from our faq huh? NOOOOPE!");
            }

            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            _context.FrequentlyAskedQuestions.Remove(faq);
            _context.SaveChanges();

            return Ok(id);

        }
    }
}

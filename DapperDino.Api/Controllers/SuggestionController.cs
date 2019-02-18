using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [Route("api/[controller]")]
    public class SuggestionController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructor

        public SuggestionController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        #endregion


        // GET api/suggestion
        [HttpGet]
        public IEnumerable<Suggestion> Get()
        {
            return _context.Suggestions.Include(x => x.DiscordUser).ToArray();
        }

        // GET api/suggestion/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var suggestion = _context.Suggestions.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return Json(suggestion);
        }

        // POST api/suggestion
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]Suggestion value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add items to our suggestions huh? NOOOOPE!");
            }

            if (!TryValidateModel(value)) return StatusCode(500, ModelState);

            if (value.DiscordUser == null)
            {
                ModelState.AddModelError("DiscordUser", "DiscordUser is required to identify the user that made the suggestion");

                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(value.DiscordUser.DiscordId))
            {
                return NotFound();
            }
            
            var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.DiscordId == value.DiscordUser.DiscordId);

            if (discordUser == null)
            {
                discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.DiscordUser.DiscordId);

                if (discordUser == null)
                {
                    discordUser = new DiscordUser()
                    {
                        DiscordId = value.DiscordUser.DiscordId,
                        Name = value.DiscordUser.Name,
                        Username = value.DiscordUser.Username
                    };
                    _context.DiscordUsers.Add(discordUser);
                    _context.SaveChanges();
                }

                return StatusCode(500, "Please contact the administrator, you have multiple accounts in the database, this shouldn't be possible!");
            }

            value.DiscordUserId = discordUser.Id;

            _context.Suggestions.Add(value);
            _context.SaveChanges();
            
            value.DiscordUser = discordUser;

            await _hubContext.Clients.All.SendAsync("Suggestion", value);

            return Created(Url.Action("Get", new { id = value.Id }), value);
        }

        // PUT api/suggestion/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody]Suggestion value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add items to our suggestions huh? NOOOOPE!");
            }

            var suggestion = _context.Suggestions.FirstOrDefault(x => x.Id == id);

            if (suggestion == null) return NotFound();

            suggestion.Description = value.Description;
            suggestion.Type = value.Type;
            suggestion.Status = value.Status;

            _context.SaveChanges();

            return Ok(suggestion);
        }

        // DELETE api/suggestion/5
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
                return StatusCode(403, "Trying to add items to our suggestions huh? NOOOOPE!");
            }

            var suggestion = _context.Suggestions.FirstOrDefault(x => x.Id == id);

            if (suggestion == null) return NotFound();

            _context.Suggestions.Remove(suggestion);
            _context.SaveChanges();

            return Ok(id);

        }
    }
}
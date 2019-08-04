using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DapperDino.Core.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class DiscordUserController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor(s)

        public DiscordUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        #endregion


        // GET api/discorduser/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(string id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Id != id && !await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to look into others accounts eh? Not gonna happen bud.");
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.Id == user.DiscordUserId);

            if (discordUser == null)
            {
                return NotFound("Discord user not found, use /api/account/registerdiscord to create one.");
            }

            return Json(discordUser);
        }



        // POST api/discorduser
        /*[HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]DiscordUser value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            _context.FrequentlyAskedQuestions.Add(value);
            _context.SaveChanges();

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
                        x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link)); ;
                }

                value.ResourceLinkId = resourceLink.Id;
                _context.SaveChanges();
            }

            return Created(Url.Action("Get", new { id = value.Id }), value);
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
        }*/

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
                return StatusCode(403, "Trying to add items to our tickets huh? NOOOOPE!");
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.Id == id);

            if (discordUser == null) return NotFound();

            _context.DiscordUsers.Remove(discordUser);
            _context.SaveChanges();

            return Ok(id);

        }

        [Authorize]
        [HttpGet("SyncUser/{id}")]
        public async Task<IActionResult> SyncUser(string id)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return BadRequest();
            }

            if (!await _userManager.IsInRoleAsync(loggedInUser, RoleNames.Admin))
            {
                return BadRequest();
            }

            var user = await _context.ApplicationUsers.Include(x => x.DiscordUser).FirstOrDefaultAsync(x => x.DiscordUser.DiscordId == id);
            if (user == null)
            {
                return BadRequest();
            }
            using (Bot bot = new Bot(_configuration))
            {
                bot.RunAsync();

                var client = bot.GetClient();

                var guild = await client.GetGuildAsync(446710084156915722);
                var member = await guild.GetMemberAsync(ulong.Parse(id));
                var roles = await _userManager.GetRolesAsync(user);
                var counted = member.Roles.Count();
                var toRemove = counted <= 0 ? roles : roles.Where(x => !member.Roles.Any(y => ("discord_" + y.Name.ToLower()) == x) && x.StartsWith("discord_"));
                if (counted > 0)
                {
                    var toAdd = member.Roles.Where(x => !roles.Any(y => y == ("discord_" + x.Name.ToLower())));

                    if (toAdd.Count() > 0)
                    {
                        foreach (var role in toAdd)
                        {
                            if (await _roleManager.FindByNameAsync("discord_" + role.Name.ToLower()) == null)
                            {
                                await _roleManager.CreateAsync(new IdentityRole("discord_" + role.Name.ToLower()));
                            }
                            if (!await _userManager.IsInRoleAsync(user, "discord_" + role.Name.ToLower()))
                            {
                                await _userManager.AddToRoleAsync(user, "discord_" + role.Name.ToLower());
                            }
                        }
                    }
                }

                if (toRemove.Count() > 0)
                {
                    foreach (var role in toRemove)
                    {
                        if (await _userManager.IsInRoleAsync(user, role))
                        {
                            await _userManager.RemoveFromRoleAsync(user, role);
                        }
                    }
                }

                var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId);

                discordUser.Username = member.Username;
                await _context.SaveChangesAsync();
            }

            return Ok(id);
        }
    }
}

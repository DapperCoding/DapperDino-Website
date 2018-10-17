using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class DiscordUserController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        #region Constructor(s)

        public DiscordUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion

        // GET api/faq
        [HttpGet]
        public IEnumerable<DiscordUser> Get()
        {
            return _context.DiscordUsers.ToArray();
        }

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
        }*/

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

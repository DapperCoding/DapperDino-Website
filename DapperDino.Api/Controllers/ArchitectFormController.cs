using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Models.Forms;
using DapperDino.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("/api/forms/architect")]
    [Authorize]
    public class ArchitectFormController : FormBaseController
    {
        private readonly ArchitectFormRepository architectFormRepository;
        public ArchitectFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
            architectFormRepository = new ArchitectFormRepository(_context);
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return Unauthorized("Admin only, go away.");
            }

            return Json(architectFormRepository.GetAll().Take(100).ToList());
        }

        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return Unauthorized("Admin only, go away.");
            }

            var form = architectFormRepository.GetById(id);
            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("ForDiscordUser/{discordId}")]
        public async Task<IActionResult> GetForDiscordUser(string discordId)
        {
            var form = await architectFormRepository.GetForDiscordUser(discordId).FirstOrDefaultAsync();

            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("DiscordUserPerspective/{discordId}")]
        public async Task<IActionResult> GetDiscordUserPerspective(string discordId)
        {
            List<ArchitectForm> forms = null;

            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            var loggedInUserRoles = await _userManager.GetRolesAsync(loggedInUser);

            if (loggedInUserRoles.IndexOf(RoleNames.Admin) < 0)
            {
                return BadRequest();
            }

            var discordUser = await _context.ApplicationUsers.Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == discordId).FirstOrDefaultAsync();

            if (discordUser == null)
            {
                return BadRequest();
            }

            var discordUserRoles = await _userManager.GetRolesAsync(discordUser);
            if (discordUserRoles.Where(x => x.ToLower() == "discord_admin" || x.ToLower() == "discord_architect").Count() > 0)
            {
                forms = architectFormRepository.GetAll().Include(x => x.Replies).ThenInclude(x => x.DiscordMessage).Take(100).ToList();
            }
            else
            {
                forms = architectFormRepository.GetForDiscordUser(discordId).ToList();
            }


            return Json(forms);
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(ArchitectForm form)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            var loggedInUserRoles = await _userManager.GetRolesAsync(loggedInUser);

            if (loggedInUserRoles.IndexOf(RoleNames.Admin) < 0)
            {
                return BadRequest();
            }

            var dbForm = await architectFormRepository.GetById(form.Id);

            if (dbForm == null)
            {
                return NotFound();
            }

            _context.Update(dbForm);

            dbForm.Motivation = form.Motivation;
            dbForm.PreviousIdeas = form.PreviousIdeas;
            dbForm.DevelopmentExperience = form.DevelopmentExperience;

            await _context.SaveChangesAsync();

            return Ok(dbForm);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(ArchitectFormModel formModel)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null)
            {
                return Unauthorized();
            }

            var loggedInUserRoles = await _userManager.GetRolesAsync(loggedInUser);

            if (loggedInUserRoles.IndexOf(RoleNames.Admin) < 0)
            {
                return BadRequest();
            }

            if (
                string.IsNullOrWhiteSpace(formModel.DiscordId) ||
                string.IsNullOrWhiteSpace(formModel.DevelopmentExperience) ||
                string.IsNullOrWhiteSpace(formModel.Motivation) ||
                string.IsNullOrWhiteSpace(formModel.PreviousIdeas)
                )
            {
                return BadRequest();
            }

            var discordUser = await _context.ApplicationUsers.Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == formModel.DiscordId).FirstOrDefaultAsync();

            if (discordUser == null)
            {
                return BadRequest();
            }

            var discordUserRoles = await _userManager.GetRolesAsync(discordUser);
            if (discordUserRoles.Where(x => x.ToLower() == "discord_admin" || x.ToLower() == "discord_architect").Count() <= 0)
            {
                return BadRequest();
            }

            var form = new ArchitectForm();

            form.Age = formModel.Age;
            form.DevelopmentExperience = formModel.DevelopmentExperience;
            form.DiscordId = discordUser.DiscordUser.Id;
            form.Motivation = formModel.Motivation;
            form.PreviousIdeas = formModel.PreviousIdeas;

            await architectFormRepository.Create(form);

            return Json(form);

        }
    }
}

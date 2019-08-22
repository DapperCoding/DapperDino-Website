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
    public class TeacherFormController : FormBaseController
    {
        private readonly TeacherFormRepository teacherFormRepository;
        public TeacherFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
            teacherFormRepository = new TeacherFormRepository(_context);
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

            return Json(teacherFormRepository.GetAll().Take(100).ToList());
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

            var form = await teacherFormRepository.GetById(id);
            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("ForDiscordUser/{discordId}")]
        public async Task<IActionResult> GetForDiscordUser(string discordId)
        {
            var form = await teacherFormRepository.GetForDiscordUser(discordId).FirstOrDefaultAsync();

            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("DiscordUserPerspective/{discordId}")]
        public async Task<IActionResult> GetDiscordUserPerspective(string discordId)
        {
            List<TeacherForm> forms = null;

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
                forms = teacherFormRepository.GetAll().Include(x => x.Replies).ThenInclude(x => x.DiscordMessage).Take(100).ToList();
            }
            else
            {
                forms = teacherFormRepository.GetForDiscordUser(discordId).ToList();
            }


            return Json(forms);
        }

        [Route("OpenInPerspective/{discordId}")]
        public async Task<IActionResult> OpenInPerspective(string discordId)
        {
            List<TeacherForm> forms = null;

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
                forms = teacherFormRepository.GetAll().Where(x=>x.Status == ApplicationFormStatus.Open).Include(x => x.Replies).ThenInclude(x => x.DiscordMessage).Take(100).ToList();
            }
            else
            {
                forms = teacherFormRepository.GetForDiscordUser(discordId).Where(x => x.Status == ApplicationFormStatus.Open).ToList();
            }


            return Json(forms);
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(TeacherForm form)
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

            var dbForm = await teacherFormRepository.GetById(form.Id);

            if (dbForm == null)
            {
                return NotFound();
            }

            _context.Update(dbForm);

            dbForm.Motivation = form.Motivation;
            dbForm.DevelopmentExperience = form.DevelopmentExperience;
            dbForm.GithubLink = form.GithubLink;
            dbForm.ProjectLinks = form.ProjectLinks;
            dbForm.TeachingExperience = form.TeachingExperience;

            await _context.SaveChangesAsync();

            return Ok(dbForm);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]TeacherFormModel formModel)
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
                string.IsNullOrWhiteSpace(formModel.GithubLink) ||
                string.IsNullOrWhiteSpace(formModel.ProjectLinks) ||
                string.IsNullOrWhiteSpace(formModel.TeachingExperience)
                )
            {
                return BadRequest();
            }

            var discordUser = await _context.ApplicationUsers.Include(x => x.DiscordUser).Where(x => x.DiscordUser.DiscordId == formModel.DiscordId).FirstOrDefaultAsync();

            if (discordUser == null)
            {
                return BadRequest();
            }

            //var discordUserRoles = await _userManager.GetRolesAsync(discordUser);
            //if (discordUserRoles.Where(x => x.ToLower() == "discord_admin" || x.ToLower() == "discord_architect").Count() <= 0)
            //{
            //    return BadRequest();
            //}

            var form = new TeacherForm();

            form.Age = formModel.Age;
            form.DevelopmentExperience = formModel.DevelopmentExperience;
            form.DiscordId = discordUser.DiscordUser.Id;
            form.Motivation = formModel.Motivation;
            form.GithubLink = formModel.GithubLink;
            form.ProjectLinks = formModel.ProjectLinks;
            form.TeachingExperience = formModel.TeachingExperience;


            await teacherFormRepository.Create(form);

            return Json(form);

        }
    }
}

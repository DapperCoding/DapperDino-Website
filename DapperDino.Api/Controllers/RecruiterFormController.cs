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
    [Route("/api/forms/recruiter")]
    [Authorize]
    public class RecruiterFormController : FormBaseController
    {
        private readonly RecruiterFormRepository recruiterFormRepository;
        public RecruiterFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
            recruiterFormRepository = new RecruiterFormRepository(_context);
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

            return Json(recruiterFormRepository.GetAll().Take(100).ToList());
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

            var form = await recruiterFormRepository.GetById(id);
            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("ForDiscordUser/{discordId}")]
        public async Task<IActionResult> GetForDiscordUser(string discordId)
        {
            var form = await recruiterFormRepository.GetForDiscordUser(discordId).FirstOrDefaultAsync();

            if (form == null) return NotFound();

            return Json(form);
        }

        [Route("DiscordUserPerspective/{discordId}")]
        public async Task<IActionResult> GetDiscordUserPerspective(string discordId)
        {
            List<RecruiterForm> forms = null;

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
                forms = recruiterFormRepository.GetAll().Include(x => x.Replies).ThenInclude(x => x.DiscordMessage).Take(100).ToList();
            }
            else
            {
                forms = recruiterFormRepository.GetForDiscordUser(discordId).ToList();
            }


            return Json(forms);
        }

        [Route("OpenInPerspective/{discordId}")]
        public async Task<IActionResult> OpenInPerspective(string discordId)
        {
            List<RecruiterForm> forms = null;

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
                forms = recruiterFormRepository.GetAll().Where(x=>x.Status == ApplicationFormStatus.Open).Include(x => x.Replies).ThenInclude(x => x.DiscordMessage).Take(100).ToList();
            }
            else
            {
                forms = recruiterFormRepository.GetForDiscordUser(discordId).Where(x => x.Status == ApplicationFormStatus.Open).ToList();
            }


            return Json(forms);
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(RecruiterForm form)
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

            var dbForm = await recruiterFormRepository.GetById(form.Id);

            if (dbForm == null)
            {
                return NotFound();
            }

            _context.Update(dbForm);

            dbForm.Motivation = form.Motivation;
            dbForm.DevelopmentExperience = form.DevelopmentExperience;
            dbForm.GithubLink = form.GithubLink;
            dbForm.ProjectLinks = form.ProjectLinks;

            await _context.SaveChangesAsync();

            return Ok(dbForm);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]RecruiterFormModel formModel)
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
                string.IsNullOrWhiteSpace(formModel.DevelopmentReviewingExperience) ||
                string.IsNullOrWhiteSpace(formModel.RecruitingExperience)
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

            var form = new RecruiterForm();

            form.Age = formModel.Age;
            form.DevelopmentExperience = formModel.DevelopmentExperience;
            form.DiscordId = discordUser.DiscordUser.Id;
            form.Motivation = formModel.Motivation;
            form.GithubLink = formModel.GithubLink;
            form.ProjectLinks = formModel.ProjectLinks;
            form.RecruitingExperience = formModel.RecruitingExperience;
            form.DevelopmentReviewingExperience = formModel.DevelopmentReviewingExperience;


            await recruiterFormRepository.Create(form);

            return Json(form);

        }
    }
}

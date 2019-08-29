using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Models.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Api.Controllers
{
    [Route("/Api/Forms")]
    public class ApplicationFormController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Starts interview based on form id and recruiter discord id (NOT DB ID)
        /// </summary>
        /// <param name="formType">[URL] the form type (FormTypeNames)</param>
        /// <param name="formId">[URL] the form id</param>
        /// <param name="discordId">[URL] discord id from discord (NOT DB ID)</param>
        /// <param name="updateModel">Only required property to fill is reason</param>
        /// <returns></returns>
        [Route("{formType}/{formId}/Interview/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Interview(string formType, int formId, string discordId, [FromBody]FormStatusUpdateModel updateModel)
        {
            return await UpdateStatus(formType, formId, discordId, updateModel, ApplicationFormStatus.Interviewing);
        }

        /// <summary>
        /// Accepts the person to the team based on form id and recruiter discord id (NOT DB ID)
        /// </summary>
        /// <param name="formType">[URL] the form type (FormTypeNames)</param>
        /// <param name="formId">[URL] the form id</param>
        /// <param name="discordId">[URL] discord id from discord (NOT DB ID)</param>
        /// <param name="updateModel">Only required property to fill is reason</param>
        /// <returns></returns>
        [Route("{formType}/{formId}/Accept/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Accept(string formType, int formId, string discordId, [FromBody]FormStatusUpdateModel updateModel)
        {
            return await UpdateStatus(formType, formId, discordId, updateModel, ApplicationFormStatus.Accepted);
        }

        /// <summary>
        /// Rejects the person based on form id and recruiter discord id (NOT DB ID)
        /// </summary>
        /// <param name="formType">[URL] the form type (FormTypeNames)</param>
        /// <param name="formId">[URL] the form id</param>
        /// <param name="discordId">[URL] discord id from discord (NOT DB ID)</param>
        /// <param name="updateModel">Only required property to fill is reason</param>
        /// <returns></returns>
        [Route("{formType}/{formId}/Reject/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Reject(string formType, int formId, string discordId, [FromBody]FormStatusUpdateModel updateModel)
        {
            return await UpdateStatus(formType, formId, discordId, updateModel, ApplicationFormStatus.Denied);
        }

        /// <summary>
        /// Leaves the interview and opens back up to other recruiters based on form id and recruiter discord id (NOT DB ID)
        /// </summary>
        /// <param name="formType">[URL] the form type (FormTypeNames)</param>
        /// <param name="formId">[URL] the form id</param>
        /// <param name="discordId">[URL] recruiter discord id from discord (NOT DB ID)</param>
        /// <param name="updateModel">Only required property to fill is reason</param>
        /// <returns></returns>
        [Route("{formType}/{formId}/Leave/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Leave(string formType, int formId, string discordId, [FromBody]FormStatusUpdateModel updateModel)
        {
            return await UpdateStatus(formType, formId, discordId, updateModel, ApplicationFormStatus.Open);
        }

        /// <summary>
        /// Updates the status based on 
        /// </summary>
        /// <param name="formType">The form type (FormTypeNames)</param>
        /// <param name="formId">The form id</param>
        /// <param name="discordId">The recruiter discord id from discord (NOT DB ID)</param>
        /// <param name="updateModel">Only required property is reason</param>
        /// <param name="status">The status to set the form to</param>
        /// <returns></returns>
        private async Task<IActionResult> UpdateStatus(string formType, int formId, string discordId, [FromBody]FormStatusUpdateModel updateModel, ApplicationFormStatus status)
        {
            // Check inputs
            if (string.IsNullOrWhiteSpace(formType))
            {
                return BadRequest();
            }

            var lowered = formType.ToLower();

            if (formId <= 0)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(discordId))
            {
                return BadRequest();
            }

            // Get user
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            // Validate that the reason is filled
            if (updateModel == null || string.IsNullOrWhiteSpace(updateModel.Reason))
            {
                return BadRequest();
            }

            // Check if API user is admin
            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to update the status huh? NOOOOPE!");
            }

            // Get discord user/recruiter
            var recruiter = await _context.ApplicationUsers.Include(x => x.DiscordUser).SingleOrDefaultAsync(x => x.DiscordUser.DiscordId == discordId);

            if (recruiter == null)
            {
                return BadRequest();
            }

            //Make sure discord user is recruiter
            if (!await _userManager.IsInRoleAsync(recruiter, RoleNames.DiscordRecruiter) && !await _userManager.IsInRoleAsync(recruiter, RoleNames.Admin) && !await _userManager.IsInRoleAsync(recruiter, RoleNames.DiscordAdmin))
            {
                return BadRequest();
            }

            // Get form
            FormBase form = await GetForm(lowered, formId);

            if (form == null)
            {
                return BadRequest();
            }

            if (form.Status == ApplicationFormStatus.Interviewing && status == ApplicationFormStatus.Interviewing)
            {
                return BadRequest();
            }

            // Set status
            form.Status = status;

            // Update form in db
            await _context.SaveChangesAsync();

            // Add form status update
            FormStatusUpdate statusUpdate = new FormStatusUpdate();
            _context.Add(statusUpdate);

            statusUpdate.Timestamp = DateTime.Now;
            statusUpdate.FormId = form.Id;
            statusUpdate.FormType = lowered;
            statusUpdate.DiscordId = recruiter.DiscordUserId ?? -1;
            statusUpdate.Reason = updateModel.Reason;

            // Should never happen
            if (statusUpdate.DiscordId <= 0)
            {
                return BadRequest();
            }

            // Get status update status id based on form status
            statusUpdate.StatusId = await GetStatusIdForStatus(form.Status);

            // Check if not found (should never happen)
            if (statusUpdate.StatusId <= 0)
            {
                return BadRequest();
            }

            // Store in db
            await _context.SaveChangesAsync();

            form.DiscordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == form.DiscordId);

            return Ok(form);
        }

        /// <summary>
        /// Tries to get a form based on the form type and id
        /// </summary>
        /// <param name="lowered">Make sure to input the form type in lower case</param>
        /// <param name="formId">The form id</param>
        /// <returns></returns>
        private async Task<FormBase> GetForm(string lowered, int formId)
        {
            // return null if form id <= 0 because ids can't be <= 0
            if (formId <= 0)
            {
                return null;
            }

            switch (lowered)
            {
                // Get architect form
                case ApplicationFormTypeNames.Architect:
                    {
                        return await _context.ArchitectForms.FirstOrDefaultAsync(x => x.Id == formId);
                    }
                // Get teacher form
                case ApplicationFormTypeNames.Teacher:
                    {
                        return await _context.TeacherForms.FirstOrDefaultAsync(x => x.Id == formId);
                    }
                // Get recruiter form
                case ApplicationFormTypeNames.Recruiter:
                    {
                        return await _context.RecruiterForms.FirstOrDefaultAsync(x => x.Id == formId);
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Tries to get status id from db for application form status
        /// </summary>
        /// <param name="status">The application form status</param>
        /// <returns>The id if found, else -1</returns>
        private async Task<int> GetStatusIdForStatus(ApplicationFormStatus status)
        {
            try
            {
                switch (status)
                {
                    case ApplicationFormStatus.Open:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == FormStatusUpdateNames.ExitConversation))?.Id ?? -1;
                    case ApplicationFormStatus.Denied:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == FormStatusUpdateNames.Rejected))?.Id ?? -1;
                    case ApplicationFormStatus.Accepted:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == FormStatusUpdateNames.Accepted))?.Id ?? -1;
                    case ApplicationFormStatus.Interviewing:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == FormStatusUpdateNames.JoinConversation))?.Id ?? -1;
                    default:
                        return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}

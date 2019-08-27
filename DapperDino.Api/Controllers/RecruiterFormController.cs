using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.Core;
using DapperDino.Core.Discord;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Models.Forms;
using DapperDino.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DapperDino.Api.Controllers
{
    [Route("/api/forms/recruiter")]
    [Authorize]
    public class RecruiterFormController : FormBaseController
    {
        private readonly RecruiterFormRepository recruiterFormRepository;
        private readonly IConfiguration _configuration;
        private readonly DiscordEmbedHelper _discordEmbedHelper;

        public RecruiterFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, DiscordEmbedHelper discordEmbedHelper) : base(context, userManager)
        {
            recruiterFormRepository = new RecruiterFormRepository(_context);
            _configuration = configuration;
            _discordEmbedHelper = discordEmbedHelper;
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

        [HttpPost]
        [Route("{formId}/Reply")]
        public async Task<IActionResult> Reply(int formId, [FromBody]TeacherFormReplyModel value)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add reactions to our tickets huh? NOOOOPE!");
            }

            if (string.IsNullOrWhiteSpace(value.DiscordMessage.Message))
            {
                value.DiscordMessage.Message = "EMPTY";
            }

            if (!TryValidateModel(value))
            {
                return StatusCode(500, ModelState);
            }

            var formReply = new TeacherFormReply();
            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.DiscordId);

            formReply.FormId = formId;

            formReply.DiscordMessage = new DiscordMessage();
            formReply.DiscordMessage.ChannelId = value.DiscordMessage.ChannelId;
            formReply.DiscordMessage.IsDm = value.DiscordMessage.IsDm;
            formReply.DiscordMessage.MessageId = value.DiscordMessage.MessageId;
            formReply.DiscordMessage.IsEmbed = value.DiscordMessage.IsEmbed;
            formReply.DiscordMessage.Message = value.DiscordMessage.Message;
            formReply.DiscordMessage.Timestamp = value.DiscordMessage.Timestamp;
            formReply.DiscordMessage.GuildId = value.DiscordMessage.GuildId;
            try
            {

                using (Bot bot = new Bot(_configuration))
                {
                    bot.RunAsync();

                    var client = bot.GetClient();

                    var guild = await client.GetGuildAsync(ulong.Parse(value.DiscordMessage.GuildId));
                    var channel = guild.GetChannel(ulong.Parse(value.DiscordMessage.ChannelId));
                    var message = await channel.GetMessageAsync(ulong.Parse(value.DiscordMessage.MessageId));

                    formReply.DiscordMessage = UpdateMessage(formReply.DiscordMessage, message);

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            var user = new DiscordUser();

            if (!string.IsNullOrWhiteSpace(value.DiscordId))
            {
                user = _context.DiscordUsers.SingleOrDefault(x => x.DiscordId == value.DiscordId);

                if (user == null)
                {
                    user = _context.DiscordUsers.FirstOrDefault();

                    if (user != null)
                    {
                        return BadRequest($"Please contact mick about this ID: {user.Id}  & Discord ID: {user.DiscordId} - ticket reaction");
                    }
                }
            }

            _context.TeacherFormReplies.Add(formReply);
            _context.SaveChanges();

            formReply = await _context.TeacherFormReplies
                .Include(x => x.DiscordMessage)

                .Include(x => x.DiscordMessage)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Attachments)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Color)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Footer)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Image)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Thumbnail)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Video)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Provider)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Author)
                .Include(x => x.DiscordMessage).ThenInclude(x => x.Embeds).ThenInclude(x => x.Fields)
                .SingleOrDefaultAsync(x => x.Id == formReply.Id);

            //await _hubContext.Clients.All.SendAsync("TicketReaction", reaction);

            return Created(Url.Action("Get", new { id = formReply.Id }), formReply);
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

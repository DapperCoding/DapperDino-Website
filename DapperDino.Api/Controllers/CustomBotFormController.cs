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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomBotFormController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DiscordEmbedHelper _discordEmbedHelper;

        #endregion

        #region Constructor(s)

        public CustomBotFormController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, DiscordEmbedHelper discordEmbedHelper)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _discordEmbedHelper = discordEmbedHelper;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            return Json(_context.CustomBotForms.OrderByDescending(x => x.Id).Take(100));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ById(int id)
        {
            var customBotForm = _context.CustomBotForms.FirstOrDefault(x => x.Id == id);

            if (customBotForm == null) return NotFound();

            return Json(customBotForm);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody]CustomBotFormModel model)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(appUser, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add xp to one of our users huh? NOOOOPE!");
            }

            if (model == null)
            {
                return BadRequest();
            }

            if (!TryValidateModel(model))
            {
                return BadRequest();
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == model.DiscordId);
            if (discordUser == null)
            {
                return BadRequest();
            }
            var entity = new CustomBotForm();

            _context.Add(entity);

            entity.Name = discordUser.Name;
            entity.Budget = model.Budget;
            entity.Description = model.Description;
            entity.Functionalities = model.Functionalities;
            entity.DiscordId = discordUser.Id;
            entity.Status = CustomBotFormStatus.NotLookedAt;

            _context.SaveChanges();

            return Created(Url.Action("ById", new { id = entity.Id }), entity);
        }

        [HttpPost]
        [Route("{formId}/Reply")]
        public async Task<IActionResult> Reply(int formId, [FromBody]CustomBotFormReplyModel value)
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

            var formReply = new CustomBotFormReply();
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

            _context.CustomBotFormReply.Add(formReply);
            _context.SaveChanges();

            formReply = await _context.CustomBotFormReply
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

            return Created(Url.Action("ById", new { id = formReply.Id }), formReply);
        }

        [Route("/{formId}/Abandon/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Leave(int formId, string discordId, [FromBody]string reason)
        {
            return await UpdateStatus(formId, discordId, reason, CustomBotFormStatus.Abandoned);
        }

        [Route("/{formId}/Deal/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Deal (int formId, string discordId, [FromBody]string reason)
        {
            return await UpdateStatus(formId, discordId, reason, CustomBotFormStatus.Deal);
        }

        [Route("/{formId}/NoDeal/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> NoDeal(int formId, string discordId, [FromBody]string reason)
        {
            return await UpdateStatus(formId, discordId, reason, CustomBotFormStatus.NoDeal);
        }

        [Route("/{formId}/Complete/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> Complete(int formId, string discordId, [FromBody]string reason)
        {
            return await UpdateStatus(formId, discordId, reason, CustomBotFormStatus.Done);
        }

        [Route("/{formId}/TalkTo/{discordId}")]
        [HttpPost]
        public async Task<IActionResult> TalkTo(int formId, string discordId, [FromBody]string reason)
        {
            return await UpdateStatus(formId, discordId, reason, CustomBotFormStatus.Done);
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

        private async Task<IActionResult> UpdateStatus(int formId, string discordId, string reason, CustomBotFormStatus status)
        {
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
            if (string.IsNullOrWhiteSpace(reason))
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
            if (!await _userManager.IsInRoleAsync(recruiter, RoleNames.DiscordRecruiter))
            {
                return BadRequest();
            }

            // Get form
            var form = await _context.CustomBotForms.SingleOrDefaultAsync(x => x.Id == formId);

            if (form == null)
            {
                return BadRequest();
            }

            if (form.Status == CustomBotFormStatus.TalkingTo && status == CustomBotFormStatus.TalkingTo)
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
            statusUpdate.FormType = FormTypeNames.CustomBot;
            statusUpdate.DiscordId = recruiter.DiscordUserId ?? -1;
            statusUpdate.Reason = reason;

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

            return Ok();
        }

        private async Task<int> GetStatusIdForStatus(CustomBotFormStatus status)
        {
            try
            {
                switch (status)
                {
                    case CustomBotFormStatus.Abandoned:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.Abandoned))?.Id ?? -1;
                    case CustomBotFormStatus.Deal:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.Deal))?.Id ?? -1;
                    case CustomBotFormStatus.Done:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.Done))?.Id ?? -1;
                    case CustomBotFormStatus.InProgress:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.InProgress))?.Id ?? -1;
                    case CustomBotFormStatus.NoDeal:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.NoDeal))?.Id ?? -1;
                    case CustomBotFormStatus.NotLookedAt:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.NotLookedAt))?.Id ?? -1;
                    case CustomBotFormStatus.TalkingTo:
                        return (await _context.FormStatusUpdateStatuses.SingleOrDefaultAsync(x => x.Name == CustomBotFormStatusUpdateNames.TalkingTo))?.Id ?? -1;
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
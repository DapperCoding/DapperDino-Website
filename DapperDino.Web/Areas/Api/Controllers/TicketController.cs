using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Constants;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Areas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DapperDino.Jobs;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Areas.Api.Controllers
{
    [Route("/Api/Tickets")]
    [Authorize]
    public class TicketController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        public TicketController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] int id)
        {
            var ticket = await _context.Tickets
                .Include(x => x.Applicant)
                .Include(x => x.Assignees)
                .Include(x => x.Reactions).ThenInclude(x => x.DiscordMessage)
                .Include(x => x.Reactions).ThenInclude(x => x.From)
                .SingleOrDefaultAsync(x => x.Id == id);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            if (ticket == null)
            {
                return BadRequest();
            }

            if (
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp) ||
                ticket.ApplicantId == user.DiscordUser.Id)
            {
                return Json(ticket);
            }

            return BadRequest();
        }

        [HttpGet("GetApiToken")]
        public async Task<IActionResult> GetApiToken()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            return Json(user.WebsiteApiToken);
        }

        [HttpGet("GetClosedTickets")]
        public async Task<IActionResult> GetClosedTickets()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            List<Ticket> tickets = null;

            if (
               await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
               await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp))
            {
                tickets = await _context.Tickets
                    .Include(x => x.Assignees)
                    .Include(x => x.Reactions).ThenInclude(x => x.DiscordMessage)
                    .Include(x => x.Reactions).ThenInclude(x => x.From)
                    .Include(x => x.Applicant)
                    .Where(x => x.Status == TicketStatus.Closed)
                    .ToListAsync();
            }
            else
            {

                tickets = await _context.Tickets
                    .Include(x => x.Assignees)
                    .Include(x => x.Reactions).ThenInclude(x => x.DiscordMessage)
                    .Include(x => x.Reactions).ThenInclude(x => x.From)
                    .Include(x => x.Applicant)
                    .Where(x => x.ApplicantId == discordUser.Id && x.Status == TicketStatus.Closed)
                    .ToListAsync();
            }


            if (tickets == null)
            {
                return BadRequest();
            }

            return Json(tickets);
        }

        [HttpGet("GetOpenTickets")]
        public async Task<IActionResult> GetOpenTickets()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            Ticket[] tickets = null;

            if (
               await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
               await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp))
            {
                tickets = await _context.Tickets
                    .Include(x => x.Assignees)
                    .Include(x => x.Reactions)
                    .Include(x => x.Applicant)
                    .Where(x => x.Status == TicketStatus.Open || x.Status == TicketStatus.InProgress)
                    .ToArrayAsync();
            }
            else
            {
                tickets = await _context.Tickets
                    .Include(x => x.Assignees)
                    .Include(x => x.Reactions)
                    .Include(x => x.Applicant)
                    .Where(x => x.ApplicantId == discordUser.Id && (x.Status == TicketStatus.Open || x.Status == TicketStatus.InProgress))
                    .ToArrayAsync();
            }

            if (tickets == null)
            {
                return BadRequest();
            }

            return Json(tickets);
        }

        [HttpGet("GetTicketActions/{id}")]
        public async Task<IActionResult> GetTicketActions(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            var ticket = await _context.Tickets.Include(x => x.Assignees).SingleOrDefaultAsync(x => x.Id == id);

            var isAdmin = await _userManager.IsInRoleAsync(user, RoleNames.Admin);
            var isH2h = await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp);

            var actions = new List<TicketAction>();

            if (isAdmin)
            {
                if (ticket.Status == TicketStatus.Closed)
                {
                    actions.Add(new TicketAction() { Action = "Reopen", Url = "" });
                }
                else if (ticket.Status == TicketStatus.Open || ticket.Status == TicketStatus.InProgress)
                {

                    if (_context.TicketReactions.Any(x => x.TicketId == ticket.Id && x.FromId == discordUser.Id) || ticket.Assignees.Any(x => x.DiscordUserId == discordUser.Id))
                    {
                        actions.Add(new TicketAction() { Action = "Close", Url = $"/Api/TicketCommands/Close/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "ForceClose", Url = $"/Api/TicketCommands/ForceClose/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Debugger", Url = $"/Api/TicketCommands/Debugger/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Code", Url = $"/Api/TicketCommands/Code/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Error", Url = $"/Api/TicketCommands/Error/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Voice Fix (ytdl)", Url = $"/Api/TicketCommands/YtdlFix/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Summon", Url = $"/Api/TicketCommands/Summon/{ticket.Id}/" });
                        actions.Add(new TicketAction() { Action = "Free", Url = $"/Api/TicketCommands/Free/{ticket.Id}/" });
                    }
                    else
                    {
                        actions.Add(new TicketAction() { Action = "Accept", Url = $"/Api/TicketCommands/Accept/{ticket.Id}" });
                    }

                }

                // summon, free
            }
            else if (isH2h)
            {
                // (accept or forceClose), debugger, code, error, ytdlFix

                if (ticket.Status == TicketStatus.Closed)
                {
                    actions.Add(new TicketAction() { Action = "Reopen", Url = "" });
                }
                else if (ticket.Status == TicketStatus.Open || ticket.Status == TicketStatus.InProgress)
                {

                    if (_context.TicketReactions.Any(x => x.TicketId == ticket.Id && x.FromId == discordUser.Id))
                    {
                        actions.Add(new TicketAction() { Action = "Close", Url = $"/Api/TicketCommands/Close/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "ForceClose", Url = $"/Api/TicketCommands/ForceClose/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Debugger", Url = $"/Api/TicketCommands/Debugger/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Code", Url = $"/Api/TicketCommands/Code/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Error", Url = $"/Api/TicketCommands/Error/{ticket.Id}" });
                        actions.Add(new TicketAction() { Action = "Voice Fix (ytdl)", Url = $"/Api/TicketCommands/YtdlFix/{ticket.Id}" });
                    }
                    else
                    {
                        actions.Add(new TicketAction() { Action = "Accept", Url = $"/Api/TicketCommands/Accept/{ticket.Id}" });
                    }

                }
            }
            else if (ticket.ApplicantId == discordUser.Id)
            {
                actions.Add(new TicketAction() { Action = "Debugger", Url = $"/Api/TicketCommands/Debugger/{ticket.Id}" });

                if (ticket.Status == TicketStatus.Open || ticket.Status == TicketStatus.InProgress)
                {
                    actions.Add(new TicketAction() { Action = "Close", Url = $"/Api/TicketCommands/Close/{ticket.Id}" });
                }
            }


            return Json(actions);
        }


        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] NotSentTicketMessage message)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            message.FromId = discordUser.Id;
            message.Username = discordUser.Username;
            message.DiscordId = discordUser.DiscordId;

            _hubContext.Clients.All.SendAsync("AddTicketReaction", message);

            return Json(true);
        }

        [HttpGet("GetCurrentDiscordUser")]
        public async Task<IActionResult> GetCurrentDiscordUser()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null || !user.DiscordUserId.HasValue)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.Id == user.DiscordUserId.Value);

            if (discordUser == null)
            {
                return BadRequest();
            }

            return Json(discordUser);
        }

    }
}
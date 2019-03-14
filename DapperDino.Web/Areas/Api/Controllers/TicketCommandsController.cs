using DapperDino.Areas.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Api.Controllers
{
    [Route("/Api/TicketCommands")]
    [Authorize]
    public class TicketCommandsController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        public TicketCommandsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet("Close/{id}")]
        public async Task<IActionResult> Close(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            // Get ticket
            var ticket = _context.Tickets.Include(x => x.Applicant).SingleOrDefault(x => x.Id == id);

            // Check if user is creator
            if (ticket.ApplicantId == user.DiscordUserId)
            {
                // close ticket
                ticket.Status = TicketStatus.Closed;

                _context.SaveChanges();

                await _hubContext.Clients.All.SendAsync("CloseTicket", id);

                return Json(true);

            }
            else if (
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
                )
            {
                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                if (discordUser == null) return Json(false);

                // Send close embed
                await _hubContext.Clients.All.SendAsync("CloseTicketEmbed", new CloseTicketEmbed() { Ticket = ticket, User = discordUser });
            }

            return Json(true);
        }

        [HttpGet("ForceClose/{id}")]
        public async Task<IActionResult> ForceClose(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            // Get ticket
            var ticket = _context.Tickets.SingleOrDefault(x => x.Id == id);

            // Check if user is creator or admin/h2h
            if (
                ticket.ApplicantId == user.DiscordUserId ||
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp))
            {
                // close ticket
                ticket.Status = TicketStatus.Closed;

                // save db
                _context.SaveChanges();

                // send message to bot
                await _hubContext.Clients.All.SendAsync("CloseTicket", id);

                return Json(true);

            }

            return Json(true);
        }

        [HttpGet("Accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {


            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            // Get ticket
            var ticket = _context.Tickets.Include(x => x.Applicant).Include(x => x.Assignees).SingleOrDefault(x => x.Id == id);

            // Check if user is creator
            if (ticket.ApplicantId == user.DiscordUserId)
            {
                // can't accept your own ticket?
                return Json(false);
            }
            else if (
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
                )
            {
                // Set ticket to in progress
                ticket.Status = TicketStatus.InProgress;

                _context.SaveChanges();

                // Get discord user from db
                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                // If not exists, return false
                if (discordUser == null) return Json(false);

                // If assignees is null (0 assigned members)
                if (ticket.Assignees == null) ticket.Assignees = new List<TicketUser>();


                // If discord user not already assigned
                if (!ticket.Assignees.Any(x => x.DiscordUserId == discordUser.Id && x.TicketId == ticket.Id))
                {
                    // add to db
                    ticket.Assignees.Add(new TicketUser() { DiscordUserId = discordUser.Id, TicketId = ticket.Id });

                    _context.SaveChanges();

                    discordUser.TicketUsers = null;
                    ticket.Assignees = null;
                    // Send message to bot
                    await _hubContext.Clients.All.SendAsync("AcceptTicket", new AcceptTicketEmbed() { Ticket = ticket, User = discordUser });
                }




                return Json(new { Refresh = true });
            }

            return Json(false);
        }

        [HttpGet("Debugger/{id}")]
        public async Task<IActionResult> Debugger(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            var ticket = await _context.Tickets.Include(x => x.Applicant).SingleOrDefaultAsync(x => x.Id == id);

            if (ticket == null) return Json(false);

            if (
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
                )
            {

                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                if (discordUser == null) return Json(false);

                await _hubContext.Clients.All.SendAsync("Debugger", new AcceptTicketEmbed() { Ticket = ticket, User = discordUser });

                return Json(true);
            }

            return Json(true);
        }

        [HttpGet("Code/{id}")]
        public async Task<IActionResult> Code(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            // Get ticket
            var ticket = _context.Tickets.Include(x => x.Applicant).SingleOrDefault(x => x.Id == id);

            if (
               await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
               await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
               )
            {

                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                if (discordUser == null) return Json(false);

                await _hubContext.Clients.All.SendAsync("Code", new AcceptTicketEmbed() { Ticket = ticket, User = discordUser });

                return Json(true);
            }

            return Json(true);
        }

        [HttpGet("Error/{id}")]
        public async Task<IActionResult> Error(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json(false);

            // Get ticket
            var ticket = _context.Tickets.SingleOrDefault(x => x.Id == id);

            if (
                await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
                )
            {

                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                if (discordUser == null) return Json(false);

                await _hubContext.Clients.All.SendAsync("Error", new AcceptTicketEmbed() { Ticket = ticket, User = discordUser });

                return Json(true);
            }

            return Json(true);
        }

        [HttpGet("YtdlFix/{id}")]
        public async Task<IActionResult> YtdlFix(int id)
        {
            // get user
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Json("User not found");

            // Get ticket
            var ticket = _context.Tickets.SingleOrDefault(x => x.Id == id);

            // Check if user is creator
            if (
                 await _userManager.IsInRoleAsync(user, RoleNames.Admin) ||
                 await _userManager.IsInRoleAsync(user, RoleNames.HappyToHelp)
                 )
            {

                var discordUser = _context.DiscordUsers.SingleOrDefault(x => x.Id == user.DiscordUserId);

                if (discordUser == null) return Json(false);

                await _hubContext.Clients.All.SendAsync("YtdlFix", new AcceptTicketEmbed() { Ticket = ticket, User = discordUser });

                return Json(true);
            }

            return Json(true);
        }



        #region Private methods


        #endregion
    }
}
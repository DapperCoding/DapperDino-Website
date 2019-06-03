using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : BaseController
    {

        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly HubConnection _connection;
        private readonly IHubContext<DiscordBotHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructor(s)

        public TicketController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        #endregion

        // GET api/ticket
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {

            return _context.Tickets.ToArray();
        }

        // GET api/ticket/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ticket = _context.Tickets
                .Include(x=>x.Language)
                .Include(x=>x.Framework)
                .Include(x => x.Applicant)
                .Include(x => x.Assignees)
                .Include(x => x.Reactions).ThenInclude(x=>x.DiscordMessage)
                .FirstOrDefault(x => x.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Json(ticket);
        }

        // POST api/ticket
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]Ticket value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add items to our tickets huh? NOOOOPE!");
            }

            if (!TryValidateModel(value)) return StatusCode(500);

            if (value.Applicant == null)
            {
                ModelState.AddModelError("Applicant", "Applicant is required because of identification");

                return BadRequest(ModelState);
            }

            var ticket = new Ticket
            {
                Description = value.Description,
                Subject = value.Subject,
                Category = value.Category,
                FrameworkId = value.FrameworkId,
                LanguageId = value.LanguageId
            };

            try
            {
                _context.Tickets.Add(ticket);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           

            var applicant = _context.DiscordUsers.FirstOrDefault(x =>
                    x.DiscordId == value.Applicant.DiscordId);

            if (applicant == null)
            {
                _context.DiscordUsers.Add(value.Applicant);
                _context.SaveChanges();

                applicant = value.Applicant;
            }

            ticket.ApplicantId = applicant.Id;

            _context.SaveChanges();

            ticket.Language = _context.Proficiencies.SingleOrDefault(x=>x.Id==ticket.LanguageId);
            ticket.Framework = _context.Proficiencies.SingleOrDefault(x => x.Id == ticket.FrameworkId);
            try
            {
                /*
                 * 
                await _hubContext.Clients.Group(RoleNames.HappyToHelp).SendAsync("TicketCreated", ticket);
                await _hubContext.Clients.Group($"Ticket${ticket.Id}").SendAsync("TicketCreated", ticket);
                 */
                await _hubContext.Clients.All.SendAsync("TicketCreated", ticket);
            }
            catch (Exception e)
            {

                throw e;
            }

            return Created(Url.Action("Get", new { id = ticket.Id }), ticket);
        }

        // POST api/Ticket/AddAssignee
        [HttpPost("{ticketId}/AddAssignee")]
        [Authorize]
        public async Task<IActionResult> AddAssignee(int ticketId, [FromBody]DiscordUser value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to add items to our tickets huh? NOOOOPE!");
            }

            var ticket = _context.Tickets
                .Include(x => x.Assignees)
                    .ThenInclude(x => x.DiscordUser)
                .FirstOrDefault(x => x.Id == ticketId);

            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found.");
            }

            if (ticket.Status == TicketStatus.Closed)
            {
                return BadRequest($"Ticket with id {ticketId} is already closed");
            }

            if (ticket.Assignees.FirstOrDefault(x => x.DiscordUser.DiscordId == value.DiscordId) != null)
            {
                return BadRequest($"Ticket with id {ticketId} is already assigned to you ({value.DiscordId})");
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.DiscordId);

            if (discordUser == null)
            {
                _context.DiscordUsers.Add(value);
                _context.SaveChanges();
                discordUser = value;
            }

            ticket.Assignees.Add(new TicketUser() { TicketId = ticketId, DiscordUserId = discordUser.Id });

            _context.SaveChanges();

            return Ok(ticket);
        }

        // PUT api/ticket/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody]FrequentlyAskedQuestion value)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to edit a tickets huh? NOOOOPE!");
            }

            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null) return NotFound(value);

            ticket.Description = value.Description;

            _context.SaveChanges();

            return Ok(ticket);
        }

        // POST api/Ticket/{ticketId}/CloseTicket
        [HttpPost("{ticketId}/Close")]
        [Authorize]
        public async Task<IActionResult> CloseTicket(int ticketId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to close a ticket huh? NOOOOPE!");
            }

            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);

            ticket.Status = TicketStatus.Closed;

            _context.SaveChanges();

            return Ok(ticket);
        }

        // DELETE api/ticket/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return StatusCode(403, "Trying to delete tickets huh? NOOOOPE!");
            }

            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return Ok(id);

        }

        [Route("OpenTickets")]
        [Authorize]
        public IActionResult OpenTickets()
        {
            var tickets = _context.Tickets.Include(x=>x.Assignees).Where(x => x.Status == TicketStatus.Open || x.Status == TicketStatus.InProgress).Select(x => new { x.Id, x.Assignees.Count, x.Subject, x.Description }).ToArray();
            return Json(tickets);
        }
    }
}
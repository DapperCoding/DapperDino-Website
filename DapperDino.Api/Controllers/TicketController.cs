using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : BaseController
    {

        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public TicketController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
            var ticket = _context.Tickets.Include(x => x.Applicant).Include(x => x.AssignedTo).Include(x => x.Reactions).FirstOrDefault(x => x.Id == id);

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
            if (!TryValidateModel(value)) return StatusCode(500);

            if (value.Applicant == null)
            {
                ModelState.AddModelError("Applicant", "Applicant is required because of identification");

                return BadRequest(ModelState);
            }

            var ticket = new Ticket();

            ticket.Description = value.Description;
            ticket.Subject = value.Subject;

            if (value.AssignedTo == null)
            {
                ticket.AssignedTo = _context.DiscordUsers.FirstOrDefault(x => x.Id == 1);
            }
            else
            {
                var assignedTo = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId.Equals(value.AssignedTo.DiscordId));

                if (assignedTo != null)
                {
                    ticket.AssignedToId = assignedTo.Id;
                }
            }
            

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            

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

            await _hubContext.Clients.All.SendAsync("TicketCreated", ticket);

            return Created(Url.Action("Get", new { id = ticket.Id }), ticket);
        }

        // POST api/Ticket/AddAssignee
        [HttpPost("{ticketId}/AddAssignee")]
        [Authorize]
        public IActionResult AddAssignee(int ticketId, [FromBody]DiscordUser value)
        {
            var ticket = _context.Tickets.Include(x => x.AssignedTo).FirstOrDefault(x => x.Id == ticketId);

            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found.");
            }

            if (ticket.AssignedTo != null && ticket.AssignedTo.DiscordId == value.DiscordId)
            {
                return BadRequest($"Ticket with id {ticketId} is already assigned to you ({value.DiscordId})");
            }
            else if (ticket.AssignedTo?.Username.ToLower() != "mickie456")
            {
                return BadRequest($"Ticket with id {ticketId} is already assigned to {ticket.AssignedTo.Username} ({ticket.AssignedTo.DiscordId}).");
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.DiscordId);

            if (discordUser == null)
            {
                _context.DiscordUsers.Add(value);
                _context.SaveChanges();
                discordUser = value;
            }

            ticket.AssignedToId = discordUser.Id;
            ticket.AssignedTo = discordUser;

            _context.SaveChanges();

            return Ok(ticket);
        }

        // PUT api/ticket/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody]FrequentlyAskedQuestion value)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null) return NotFound(value);

            ticket.Description = value.Description;

            _context.SaveChanges();

            return Ok(ticket);
        }

        // POST api/Ticket/{ticketId}/CloseTicket
        [HttpPost("{ticketId}/Close")]
        [Authorize]
        public IActionResult CloseTicket(int ticketId)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == ticketId);

            ticket.Status = TicketStatus.Closed;

            _context.SaveChanges();

            return Ok(ticket);
        }

        // DELETE api/ticket/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var ticket = _context.Tickets.FirstOrDefault(x => x.Id == id);

            if (ticket == null) return NotFound();

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return Delete(id);

        }
    }
}
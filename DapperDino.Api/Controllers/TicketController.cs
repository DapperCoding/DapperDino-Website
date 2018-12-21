using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : BaseController
    {

        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        // GET api/ticket
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {
            return _context.Tickets.Include(x => x.Applicant).Include(x => x.AssignedTo).Include(x => x.Reactions).ToArray();
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
        public IActionResult Post([FromBody]Ticket value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            if (value.AssignedTo == null)
            {
                value.AssignedTo = _context.DiscordUsers.FirstOrDefault(x => x.Id == 5);
            }
            else
            {
                var v = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId.Equals(value.AssignedTo.DiscordId));

                if (v != null)
                {
                    value.AssignedToId = v.Id;
                }
            }

            _context.Tickets.Add(value);
            _context.SaveChanges();

            if (value.Applicant == null)
            {
                ModelState.AddModelError("Applicant", "Applicant is required because of identification");

                return BadRequest(ModelState);
            }

            var applicant = _context.DiscordUsers.FirstOrDefault(x =>
                    x.DiscordId.Equals(value.Applicant.DiscordId));

            if (applicant == null)
            {
                _context.DiscordUsers.Add(value.Applicant);
                _context.SaveChanges();

                applicant = _context.DiscordUsers.First(x =>
                    x.DiscordId.Equals(value.Applicant.DiscordId));
            }

            value.ApplicantId = applicant.Id;
            
            _context.SaveChanges();

            return Created(Url.Action("Get", new { id = value.Id }), value);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/ticket/reaction")]
    public class TicketReactionController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public TicketReactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        // GET api/faq/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var ticketReactions = _context.TicketReactions.Include(x=>x.Ticket).Include(x=>x.From).Where(x => x.TicketId.Equals(id));
            
            return Json(ticketReactions);
        }

        // POST api/faq
        [HttpPost]
        public IActionResult Post([FromBody]TicketReaction value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            _context.TicketReactions.Add(value);
            _context.SaveChanges();

            //if (value.ResourceLink != null)
            //{
            //    var resourceLink = _context.ResourceLinks.FirstOrDefault(x =>
            //        x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link));

            //    if (resourceLink == null)
            //    {
            //        _context.ResourceLinks.Add(new ResourceLink()
            //        {
            //            DisplayName = value.ResourceLink.DisplayName,
            //            Link = value.ResourceLink.DisplayName
            //        });
            //        _context.SaveChanges();

            //        resourceLink = _context.ResourceLinks.First(x =>
            //            x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link)); ;
            //    }

            //    value.ResourceLinkId = resourceLink.Id;
            //}

            return Created(Url.Action("Get", new { id = value.Id }), value);
        }

        // PUT api/faq/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]FrequentlyAskedQuestion value)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            faq.Answer = value.Answer;
            faq.Description = value.Description;
            faq.Question = value.Question;

            _context.SaveChanges();

            return Ok(faq);
        }

        // DELETE api/faq/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null) return NotFound();

            _context.FrequentlyAskedQuestions.Remove(faq);
            _context.SaveChanges();

            return Delete(id);

        }
    }
}

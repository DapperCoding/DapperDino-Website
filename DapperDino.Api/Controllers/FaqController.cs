using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class FaqController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public FaqController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        // GET api/faq
        [HttpGet]
        public IEnumerable<FrequentlyAskedQuestion> Get()
        {
            return _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).ToArray();
        }

        // GET api/faq/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null)
            {
                return NotFound();
            }

            return Json(faq);
        }

        // POST api/faq
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]FrequentlyAskedQuestion value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            _context.FrequentlyAskedQuestions.Add(value);
            _context.SaveChanges();

            if (value.ResourceLink != null)
            {
                var resourceLink = _context.ResourceLinks.FirstOrDefault(x =>
                    x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link));

                if (resourceLink == null)
                {
                    _context.ResourceLinks.Add(new ResourceLink()
                    {
                        DisplayName = value.ResourceLink.DisplayName,
                        Link = value.ResourceLink.DisplayName
                    });
                    _context.SaveChanges();

                    resourceLink = _context.ResourceLinks.First(x =>
                        x.DisplayName.Equals(value.ResourceLink.DisplayName) && x.Link.Equals(value.ResourceLink.Link)); ;
                }

                value.ResourceLinkId = resourceLink.Id;
                _context.SaveChanges();
            }

            return Created(Url.Action("Get", new { id = value.Id }), value);
        }

        // PUT api/faq/5
        [HttpPut("{id}")]
        [Authorize]
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

        // POST api/faq/5
        [HttpPost("/api/faq/addmessageid")]
        [Authorize]
        public IActionResult UpdateFaqMessage([FromBody]FaqMessageIdViewModel value)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == value.Id);

            if (faq == null) return NotFound($"Faq with id ${value.Id} couldn't be found");

            faq.MessageId = value.MessageId;

            _context.SaveChanges();

            return Ok(faq);
        }

        // DELETE api/faq/5
        [HttpDelete("{id}")]
        [Authorize]
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

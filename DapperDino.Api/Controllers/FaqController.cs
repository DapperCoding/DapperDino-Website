using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class FaqController : Controller
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
            return _context.FrequentlyAskedQuestions.ToArray();
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
        public IActionResult Post([FromBody]FrequentlyAskedQuestion value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            _context.FrequentlyAskedQuestions.Add(value);
            _context.SaveChanges();

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

            return Ok();

        }
    }
}

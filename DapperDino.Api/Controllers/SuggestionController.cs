using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Api.Controllers
{
    public class SuggestionController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor

        public SuggestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion


        // GET api/suggestion
        [HttpGet]
        public IEnumerable<Suggestion> Get()
        {
            return _context.Suggestions.ToArray();
        }

        // GET api/suggestion/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var suggestion = _context.Suggestions.FirstOrDefault(x => x.Id == id);

            if (suggestion == null)
            {
                return NotFound();
            }

            return Json(suggestion);
        }

        // POST api/suggestion
        [HttpPost]
        public IActionResult Post([FromBody]Suggestion value)
        {
            if (!TryValidateModel(value)) return StatusCode(500);

            if (value.DiscordUser == null || value.DiscordUser.DiscordId <= 0)
            {
                return NotFound();
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == value.DiscordUser.Id);

            if (discordUser == null)
            {
                discordUser = new DiscordUser()
                {
                    DiscordId = value.DiscordUser.DiscordId,
                    Name = value.DiscordUser.Name,
                    Username = value.DiscordUser.Username
                };
                _context.DiscordUsers.Add(discordUser);
                _context.SaveChanges();
            }
            else
            {
                discordUser.Name = value.DiscordUser.Name;
                discordUser.Username = value.DiscordUser.Username;
            }

            _context.Suggestions.Add(value);
            _context.SaveChanges();

            return Created(Url.Action("Get", new { id = value.Id }), value);
        }

        // PUT api/suggestion/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Suggestion value)
        {
            var suggestion = _context.Suggestions.FirstOrDefault(x => x.Id == id);

            if (suggestion == null) return NotFound();

            suggestion.Description = value.Description;
            suggestion.Type = value.Type;
            suggestion.Status = value.Status;

            _context.SaveChanges();

            return Ok(suggestion);
        }

        // DELETE api/suggestion/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var suggestion = _context.Suggestions.FirstOrDefault(x => x.Id == id);

            if (suggestion == null) return NotFound();

            _context.Suggestions.Remove(suggestion);
            _context.SaveChanges();

            return Ok();

        }
    }
}
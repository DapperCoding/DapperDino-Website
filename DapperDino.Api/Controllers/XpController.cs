using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class XpController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public XpController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            var discordUsers = _context.DiscordUsers.OrderBy(x => x.Xp);
            var viewModels = new List<CompactXpViewModel>();

            foreach (var discordUser in discordUsers)
            {
                viewModels.Add(new CompactXpViewModel()
                {
                    Xp = discordUser.Xp,
                    Username = discordUser.Username,
                    DiscordId = discordUser.DiscordId,
                    Level = discordUser.Level
                });
            }

            return Json(viewModels);
        }

        [HttpGet("{discordId}")]
        public IActionResult ById(string discordId)
        {
            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);

            if (discordUser == null) return NotFound("Discord user not found.");

            var viewModel = new CompactXpViewModel()
            {
                DiscordId = discordId,
                Level = discordUser.Level,
                Username = discordUser.Username,
                Xp = discordUser.Xp
            };

            return Json(viewModel);
        }

        [HttpPost("{discordId}")]
        [Authorize]
        public IActionResult Add(string discordId, [FromBody]XpViewModel model)
        {
            if (model == null)
            {
                return BadRequest("No xp model found in body.");
            }

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.DiscordId == discordId);
            var xpViewModel = new XpViewModel();

            if (discordUser == null)
            {
                discordUser = new DAL.Models.DiscordUser()
                {
                    DiscordId = model.DiscordId,
                    Level = 1,
                    Username = model.Username,
                    Xp = model.Xp
                };

                _context.DiscordUsers.Add(discordUser);
            }
            else
            {
                var nxtLvl = discordUser.Level * 200 * 1.2;

                discordUser.Xp += model.Xp;

                if (discordUser.Xp > nxtLvl)
                {
                    discordUser.Level++;
                    xpViewModel.LevelledUp = true;
                }
            }

            xpViewModel.Level = discordUser.Level;
            xpViewModel.Username = discordUser.Username;
            xpViewModel.DiscordId = discordUser.DiscordId;
            xpViewModel.Xp = discordUser.Xp;

            _context.SaveChanges();

            return Ok(xpViewModel);
        }
    }
}
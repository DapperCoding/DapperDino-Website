using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProficiencyController : BaseController
    {

        #region Fields
        private ApplicationDbContext _context;
        private DiscordUserProficiencyRepository _discordUserProficiencyRepository;
        private ProficiencyRepository _proficiencyRepository;

        #endregion

        public ProficiencyController(ApplicationDbContext context)
        {
            _context = context;
            _discordUserProficiencyRepository = new DiscordUserProficiencyRepository(_context);
            _proficiencyRepository = new ProficiencyRepository(_context);
        }
        [Route("/api/Proficiency/GetProficienciesForDiscordUser/{discordUserId}")]
        public JsonResult GetProficienciesForDiscordUser(string discordUserId)
        {
            var proficiencies = _discordUserProficiencyRepository.FindAllForDiscordUser(discordUserId);


            return Json(proficiencies);
        }

        public JsonResult GetLanguages()
        {
            var proficiencies = _proficiencyRepository.GetLanguages();


            return Json(proficiencies);
        }

        public JsonResult GetFrameworks()
        {
            var proficiencies = _proficiencyRepository.GetFrameworks();


            return Json(proficiencies);
        }
    }
}
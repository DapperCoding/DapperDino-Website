using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Repositories;
using DapperDino.Jobs;
using DapperDino.Models;
using DapperDino.Models.FaqViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    // Admin faq controller
    [Route("HappyToHelp/Proficiency")]
    public class ProficiencyController : BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly ProficiencyRepository _proficiencyRepository;
        private readonly DiscordUserProficiencyRepository _discordUserProficiencyRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructor(s)

        public ProficiencyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _proficiencyRepository = new ProficiencyRepository(_context);
            _discordUserProficiencyRepository = new DiscordUserProficiencyRepository(_context);
            _userManager = userManager;
        }

        #endregion

        #region Public Methods
        // Default page
        
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var list = _proficiencyRepository.GetAll().ToList();


            return View(list);
        }

        [Route("Edit/{id}")]
        public async Task<IActionResult> Proficiency(int id)
        {

            var proficiency = await _proficiencyRepository.GetById(id);

            if (proficiency == null)
            {
                return NotFound();
            }

            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null || !appUser.DiscordUserId.HasValue)
            {
                return Unauthorized();
            }

            var userProficiency =  await _discordUserProficiencyRepository.GetByProficiencyAndDiscordUser(id, appUser.DiscordUserId.Value);

            if (userProficiency == null)
            {

                userProficiency = new DiscordUserProficiency();

                userProficiency.DiscordUserId = appUser.DiscordUserId.Value;
                userProficiency.ProficiencyId = id;
            }

            userProficiency.Proficiency = proficiency;

            return View(userProficiency);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Proficiency(DiscordUserProficiency userProficiency)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null || !appUser.DiscordUserId.HasValue || userProficiency.DiscordUserId != appUser.DiscordUserId.Value)
            {
                return Unauthorized();
            }

            var oldProficiency = await _discordUserProficiencyRepository.GetByProficiencyAndDiscordUser(userProficiency.ProficiencyId, appUser.DiscordUserId.Value);

            if (oldProficiency != null)
            {
                oldProficiency.ProficiencyLevel = userProficiency.ProficiencyLevel;
                await _context.SaveChangesAsync();
            }
            else
            {
                userProficiency.Id = 0;
                await _discordUserProficiencyRepository.Create(userProficiency);
            }

            return RedirectToAction("Index");
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int proficiencyId)
        {
            var appUser = await _userManager.GetUserAsync(User);

            if (appUser == null || !appUser.DiscordUserId.HasValue)
            {
                return Unauthorized();
            }

            var proficiency = await _context.DiscordUserProficiencies.FirstOrDefaultAsync(x => x.DiscordUserId == appUser.DiscordUserId.Value && x.ProficiencyId == proficiencyId);

            if (proficiency != null)
            {
                return NotFound();
            }

            _context.Remove(proficiency);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

        #endregion


    }
}
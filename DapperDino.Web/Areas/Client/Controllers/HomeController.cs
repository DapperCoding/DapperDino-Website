using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Client.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Client.Controllers
{
    [Route("/Client")]
    public class HomeController: BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user =  await _userManager.GetUserAsync(User);

            var connected = user.RegisteredDiscordAccount;

            var viewModel = new HomeViewModel()
            {
                ConnectedDiscordAccount = connected
            };

            if (connected)
            {
                viewModel.Suggestions = _dbContext.Suggestions.Where(x => x.DiscordUserId == user.DiscordUserId).OrderByDescending(x=>x.Id).Take(10).ToList();
                viewModel.Tickets = _dbContext.Tickets.Where(x => x.ApplicantId == user.DiscordUserId).OrderByDescending(x => x.Id).Take(10).ToList();
            }

            return View(viewModel);
        }
    }
}

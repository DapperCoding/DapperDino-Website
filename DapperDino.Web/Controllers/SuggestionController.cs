using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Models.SuggestionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Controllers
{
    [Route("Suggestion")]
    public class SuggestionController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public SuggestionController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #endregion

      

        [Route("")]
        // Default page
        public IActionResult Index(SuggestionViewModel viewModel = null)
        {
            if (viewModel == null) viewModel = new SuggestionViewModel();

            // Return view with viewModel
            return View(viewModel);
        }

        [Route("ContactSoon")]
        // Default page
        public IActionResult ContactSoon()
        {
            return View();
        }

        [HttpPost]
        [Route("Contact")]
        public IActionResult Post(SuggestionViewModel viewModel)
        {
            if (!TryValidateModel(viewModel)) return RedirectToAction("Index", viewModel);

            var enquiry = viewModel as Suggestion;

            _context.Suggestions.Add(enquiry);
            _context.SaveChanges();

            _hubContext.Clients.All.SendAsync("Suggestion", viewModel);

            return RedirectToAction("ContactSoon");
        }
    }
}
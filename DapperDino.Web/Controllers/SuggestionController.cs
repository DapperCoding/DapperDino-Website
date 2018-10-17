using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Models.SuggestionViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Controllers
{
    public class SuggestionController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public SuggestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        // Default page
        public IActionResult Index()
        {
            // Generate indexViewModel based on current suggestions
            var viewModel = new IndexViewModel()
            {
                // Get suggestions from dbContext
                Suggestions = _context.Suggestions.Select(x => new SuggestionViewModel()
                {
                    Id = x.Id,
                    DiscordUser = x.DiscordUser,
                    Description = x.Description,
                    DiscordUserId = x.DiscordUserId,
                    Status = x.Status,
                    Type = x.Type
                }).OrderByDescending(x => x.Id)
            };

            // Return view with viewModel
            return View(viewModel);
        }
    }
}
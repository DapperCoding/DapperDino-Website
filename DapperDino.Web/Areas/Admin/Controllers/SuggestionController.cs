using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Models.SuggestionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Areas.Admin.Controllers
{
    public class SuggestionController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public SuggestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Get list of all suggestions
            var suggestions = _context.Suggestions.Include(x => x.DiscordUser).ToList();

            // Generate IndexViewModel using the suggestions list
            var viewModel = new IndexViewModel()
            {
                // Convert List<Suggestion> to List<SuggestionViewModel> TODO: Add automapper
                Suggestions = suggestions.Select(x =>

                    // Create new SuggestionViewModel for each suggestion
                    new SuggestionViewModel()
                    {
                        Description = x.Description,
                        Id = x.Id,
                        DiscordUser = x.DiscordUser,

                        DiscordUserId = x.DiscordUserId,
                        Status = x.Status,
                        Type = x.Type
                    }
                )
            };

            // Return the view -> using viewModel
            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            // Get suggestion from db
            var suggestion = _context.Suggestions.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == id);

            // Null check
            if (suggestion == null)
            {
                
                // Return not found page
                return NotFound();
            }

            // Generate viewmodel
            var viewModel = new SuggestionViewModel()
            {
                Description = suggestion.Description,
                DiscordUser = suggestion.DiscordUser,
                DiscordUserId = suggestion.DiscordUserId,
                Id = suggestion.Id,
                Status = suggestion.Status,
                Type = suggestion.Type
            };

            return View(viewModel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Models.SuggestionViewModels;
using DapperDino.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    [Route("HappyToHelp/Suggestion")]
    public class SuggestionController : BaseController
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
        public IActionResult Index()
        {
            // Get list of all suggestions
            var suggestions = _context.Suggestions.Include(x => x.DiscordUser).ToList();

            // Generate IndexViewModel using the suggestions list
            var viewModel = new IndexViewModel()
            {
                // Convert List<Suggestion> to List<SuggestionViewModel> TODO: Add automapper
                Suggestions = suggestions.Where(x => x.DiscordUser != null).Select(x =>

                      // Create new SuggestionViewModel for each suggestion
                      new SuggestionViewModel()
                      {
                          Description = x.Description,
                          Id = x.Id,
                          DiscordUser = new DiscordUser()
                          {
                              DiscordId = x.DiscordUser.DiscordId,
                              Id = x.Id,
                              Name = x.DiscordUser.Name,
                              Username = x.DiscordUser.Username,
                              Xp = x.DiscordUser.Xp,
                              Level = x.DiscordUser.Level
                          },

                          DiscordUserId = x.DiscordUserId,
                          Status = x.Status,
                          Type = x.Type
                      }
                ).ToList()
            };

            var i = viewModel.Suggestions.Select(x => x.DiscordUser);
            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("{id}")]
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

            // Return view with viewModel
            return View(viewModel);
        }

        [Route("{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, SuggestionViewModel viewModel)
        {
            // Get suggestion from db
            var suggestion = _context.Suggestions.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == id);

            // Null check
            if (suggestion == null)
            {

                // Return not found page
                return NotFound();
            }

            // Replace values in db to new values
            suggestion.Type = viewModel.Type;
            suggestion.Status = viewModel.Status;
            suggestion.Description = viewModel.Description;

            // Save changes in db
             await _context.SaveChangesAsync();

             await _hubContext.Clients.All.SendAsync("SuggestionUpdate", suggestion);

            // Generate viewmodel
            viewModel = new SuggestionViewModel()
            {
                Description = suggestion.Description,
                DiscordUser = suggestion.DiscordUser,
                DiscordUserId = suggestion.DiscordUserId,
                Id = suggestion.Id,
                Status = suggestion.Status,
                Type = suggestion.Type
            };

            // Return view with model
            return View(viewModel);
        }
    }
}

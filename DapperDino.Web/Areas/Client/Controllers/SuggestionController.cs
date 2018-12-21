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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Areas.Client.Controllers
{
    [Route("Client/Suggestion")]
    public class SuggestionController : BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;
        private readonly UserManager<DAL.Models.ApplicationUser> _userManager;

        #endregion

        #region Constructor(s)

        public SuggestionController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext, UserManager<DAL.Models.ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        #endregion

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userEntity = _context.ApplicationUsers.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == user.Id);

            // Get list of all suggestions
            var suggestions = _context.Suggestions.Where(x => x.DiscordUserId == userEntity.DiscordUserId).Include(x => x.DiscordUser).ToList();

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
            suggestion.Description = viewModel.Description;

            // Save changes in db
            await _context.SaveChangesAsync();

            //await _hubContext.Clients.All.SendAsync("SuggestionUpdate", suggestion);

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

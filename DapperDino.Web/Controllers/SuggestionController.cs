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
    public class SuggestionController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public SuggestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                Suggestions = _context.Suggestions.Select(x => new SuggestionViewModel()
                {
                    Id = x.Id,
                    DiscordUser = x.DiscordUser,
                    Description = x.Description,
                    DiscordUserId = x.DiscordUserId,
                    Status = x.Status,
                    Type = x.Type
                }).OrderBy(x=>x.Type)
            };



            return View(viewModel);
        }
    }
}
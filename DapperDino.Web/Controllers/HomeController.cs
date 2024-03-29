﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using DapperDino.Models;
using Microsoft.AspNetCore.Identity;
using DapperDino.Models.HomeViewModels;

namespace DapperDino.Controllers
{
    public class HomeController : BaseControllerBase
    {


        #region Fields

        // RoleManager used for auth
        private readonly RoleManager<IdentityRole> _roleManager;
        // UserManager used for auth
        private readonly UserManager<ApplicationUser> _userManager;
        // Shared context accessor
        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructor(s)

        public HomeController(RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        #endregion

        // Default page
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.AmountOfTicketsDone = _dbContext.Tickets.Where(x=>x.Status== TicketStatus.Closed).Count();
            viewModel.AmountOfSuggestions = _dbContext.Suggestions.Count();
            viewModel.AmountOfWebsiteUsers = _dbContext.ApplicationUsers.Count();
            return View(viewModel);
        }

        // About page
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        // Contact page
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        // Error page
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

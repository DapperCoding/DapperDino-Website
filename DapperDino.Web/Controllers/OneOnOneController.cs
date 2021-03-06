﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Models;
using DapperDino.Models.FaqViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Controllers
{
    // Faq controller
    [Route("OneOnOne")]
    public class OneOnOneController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public OneOnOneController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #endregion

        #region Public Methods
        [Route("")]
        public IActionResult Index()
        {
            var viewModel = new ApplyViewModel();

            viewModel.Age = 0;

            return View(viewModel);
        }

        [HttpPost("Post")]
        public IActionResult Post(ApplyViewModel viewModel)
        {
            if (!TryValidateModel(viewModel)) return RedirectToAction("Index", viewModel);

            var discordUser = _context.DiscordUsers.FirstOrDefault(x => x.Username == viewModel.DiscordId);

            if (discordUser == null) return RedirectToAction("Index", viewModel);

            var application = new Applicant();

            application.Age = viewModel.Age;
            application.Explanation = viewModel.Explanation;
            application.FirstName = viewModel.FirstName;
            application.LastName = viewModel.LastName;
            application.DiscordUserId = discordUser.Id;
            application.Links = viewModel.Links;

            _context.Applicants.Add(application);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home", null);
        }

        #endregion


    }
}
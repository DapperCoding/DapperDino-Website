using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Areas.Admin.Controllers
{
    // Admin faq controller
    [Route("Admin/Applicant")]
    public class ApplicantController : BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public ApplicantController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #endregion

        #region Public Methods
        // Default page
        [Route("")]
        public IActionResult Index()
        {
            // Generate viewModel based on current FAQ's 
            var viewModel = new ApplicantIndexViewModel()
            {
                // Get FAQ's from db
                Applicants = _context.Applicants.Include(x => x.DiscordUser).ToArray()
            };

            // Return view with viewmodel
            return View(viewModel);
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var faq = _context.Applicants.FirstOrDefault(x => x.Id == id);

            if (faq == null)
            {
                return NotFound("Couldn't find the Applicant you were looking for to delete.");
            }

            _context.Applicants.Remove(faq);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            // Get faq by id
            var applicant = _context.Applicants.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == id);

            // Check if null / not found in db
            if (applicant == null)
            {

                // Return 404 not found
                return NotFound("Couldn't find the FAQ you were looking for to edit.");
            }

            // Generate edit viewModel
            var viewModel = new ApplicantEditViewModel()
            {
                DiscordId = applicant.DiscordUser.DiscordId,
                Applicant = applicant
            };

            return View(viewModel);
        }

        [Route("AcceptApplicant/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            var ap = _context.Applicants.Include(x => x.DiscordUser).FirstOrDefault(x => x.Id == id);

            await _hubContext.Clients.All.SendAsync("AcceptedApplicant", new { discordId = ap.DiscordUser.DiscordId });

            // Return view with viewModel that's edited
            return RedirectToAction("Index");
        }

        [Route("DenyApplicant/{id}")]
        [HttpPost]
        public async Task<IActionResult> Deny(int id, ApplicantEditViewModel viewModel)
        {
            await _hubContext.Clients.All.SendAsync("DenyApplicant", new { discordId = viewModel.DiscordId });

            // Return view with viewModel that's edited
            return RedirectToAction("Index");
        }

        #endregion


    }
}
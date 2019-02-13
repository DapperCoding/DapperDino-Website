using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Models;
using DapperDino.Models.FaqViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    // Admin faq controller
    [Route("HappyToHelp/Faq")]
    public class FaqController : BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public FaqController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
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
            var viewModel = new IndexViewModel()
            {
                // Get FAQ's from db
                FrequentlyAskedQuestions = _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).Select(x =>
                      new FrequentlyAskedQuestionViewModel()
                      {
                          Answer = x.Answer,
                          Question = x.Question,
                          Description = x.Description,
                          Id = x.Id,
                          ResourceLink = new Models.ResourceLinkViewModel()
                          {
                              // Add resourcelink
                              Link = x.ResourceLink.Link,
                              DisplayName = x.ResourceLink.DisplayName,
                              Id = x.ResourceLinkId
                          }
                      }).ToList()
            };

            // Return view with viewmodel
            return View(viewModel);
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var faq = _context.FrequentlyAskedQuestions.FirstOrDefault(x => x.Id == id);

            if (faq == null)
            {
                return NotFound("Couldn't find the FAQ you were looking for to delete.");
            }

            _context.FrequentlyAskedQuestions.Remove(faq);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("Add")]
        public IActionResult Add()
        {

            // Generate edit viewModel
            var viewModel = new FaqEditViewModel()
            {
                ResourceLink = new Admin.Models.ResourceLinkViewModel() { }
            };

            return View(viewModel);
        }

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            // Get faq by id
            var faq = _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).FirstOrDefault(x => x.Id == id);

            // Check if null / not found in db
            if (faq == null)
            {

                // Return 404 not found
                return NotFound("Couldn't find the FAQ you were looking for to edit.");
            }

            // Generate edit viewModel
            var viewModel = new FaqEditViewModel()
            {
                Id = id,
                Answer = faq.Answer,
                Description = faq.Description,
                Question = faq.Question,
                ResourceLinkId = faq.ResourceLinkId,
                ResourceLink = new Admin.Models.ResourceLinkViewModel()
                {
                    Id = faq.ResourceLink?.Id ?? 0,
                    Link = faq.ResourceLink?.Link ?? string.Empty,
                    DisplayName = faq.ResourceLink?.DisplayName ?? string.Empty
                }
            };

            return View(viewModel);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, FaqEditViewModel viewModel)
        {
            // Used if a new resourceLink is added
            var addedResourceLink = false;

            // Get faq by id
            var faq = _context.FrequentlyAskedQuestions.Include(x=>x.DiscordMessage).FirstOrDefault(x => x.Id == id);

            // Check if null / not found in db
            if (faq == null)
            {

                // Return 404 not found
                return NotFound("Couldn't find the FAQ you were looking for to edit.");
            }

            // For now only simple editing
            faq.Answer = viewModel.Answer;
            faq.Question = viewModel.Question;
            faq.Description = viewModel.Description;


            // Resource link editing
            ResourceLink resourceLink = null;

            // Check if faq has resourceLink and viewModel.ResourceLink is filled
            if (
                viewModel.ResourceLinkId.HasValue &&
                viewModel.ResourceLinkId.Value > 0 &&
                viewModel.ResourceLink != null &&
                !string.IsNullOrWhiteSpace(viewModel.ResourceLink.Link) &&
                !string.IsNullOrWhiteSpace(viewModel.ResourceLink.DisplayName))
            {
                // Get resourceLink by id
                resourceLink = _context.ResourceLinks.FirstOrDefault(x => x.Id == viewModel.ResourceLinkId.Value);

                // If not found, return 404
                if (resourceLink == null) return NotFound("Resourcelink assigned to faq not found, please contact Mick.");

                // Edit values of ResourceLink
                resourceLink.Link = viewModel.ResourceLink.Link;
                resourceLink.DisplayName = viewModel.ResourceLink.DisplayName;
            }

            // If not, check if viewModel.ResourceLink is filled
            else if (
                viewModel.ResourceLink != null &&
                !string.IsNullOrWhiteSpace(viewModel.ResourceLink.Link) &&
                !string.IsNullOrWhiteSpace(viewModel.ResourceLink.DisplayName))
            {
                // Create new dbEntry -> ResourceLink
                resourceLink = new ResourceLink()
                {
                    Link = viewModel.ResourceLink.Link,
                    DisplayName = viewModel.ResourceLink.DisplayName
                };

                // Add to dbContext
                _context.ResourceLinks.Add(resourceLink);

                // Set to true, for adding id after resourceLink is created
                addedResourceLink = true;
            }

            // Save changes in db
            _context.SaveChanges();

            // Add resourceLink id to faq if necessary
            if (addedResourceLink && resourceLink != null)
            {
                faq.ResourceLinkId = resourceLink.Id;
                _context.SaveChanges();
            }

            if (faq.ResourceLinkId > 0)
                faq.ResourceLink = _context.ResourceLinks.FirstOrDefault(x => x.Id == faq.ResourceLinkId);

            if (faq.DiscordMessageId > 0)
                faq.DiscordMessage = _context.DiscordMessages.FirstOrDefault(x => x.Id == faq.DiscordMessageId);

            await _hubContext.Clients.All.SendAsync("FaqUpdate", faq);

            // Return view with viewModel that's edited
            return RedirectToAction("Edit", new { id });
        }

        #endregion


    }
}
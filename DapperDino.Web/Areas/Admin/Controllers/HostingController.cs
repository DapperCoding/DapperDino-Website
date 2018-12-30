using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Models;
using DapperDino.DAL;
using DapperDino.Jobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Areas.Admin.Controllers
{
    // Admin home controller
    [Route("Admin/Hosting")]
    public class HostingController : BaseController
    {

        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public HostingController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
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
            var viewModel = new HostingEnquiryIndexViewModel()
            {
                // Get Hosting enquiries from db
                List = _context.HostingEnquiries.ToList()
            };

            // Return view with viewmodel
            return View(viewModel);
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var enquiry = _context.HostingEnquiries.FirstOrDefault(x => x.Id == id);

            if (enquiry == null)
            {
                return NotFound("Couldn't find the Hosting Enquiry you were looking for to delete.");
            }

            _context.HostingEnquiries.Remove(enquiry);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("Details/{id}")]
        public IActionResult Details(int id)
        {
            // Get faq by id
            var enquiry = _context.HostingEnquiries.FirstOrDefault(x => x.Id == id);

            // Check if null / not found in db
            if (enquiry == null)
            {

                // Return 404 not found
                return NotFound("Couldn't find the Hosting Enquiry you were looking for to delete.");
            }

            return View(enquiry);
        }

        #endregion
    }
}
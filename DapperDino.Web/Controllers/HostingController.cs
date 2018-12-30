using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Models;
using DapperDino.Models.FaqViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Controllers
{
    // Faq controller
    [Route("Hosting")]
    public class HostingController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public HostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Small")]
        public IActionResult Small()
        {
            var viewModel = new HostingViewModel()
            {
                Package = "'Small'",
                PackageType = HostingType.Small
            };

            return View("Contact", viewModel);
        }

        [Route("Pro")]
        public IActionResult Pro()
        {
            var viewModel = new HostingViewModel()
            {
                Package = "'Pro'",
                PackageType = HostingType.Pro
            };

            return View("Contact", viewModel);
        }

        

        [Route("Enterprise")]
        public IActionResult Enterprise()
        {
            var viewModel = new HostingViewModel()
            {
                Package = "'Enterprise'",
                PackageType = HostingType.Enterprise
            };

            return View("Contact", viewModel);
        }

        [HttpPost("Contact")]
        public IActionResult Post(HostingViewModel viewModel)
        {
            if (!TryValidateModel(viewModel)) return RedirectToAction("Index", viewModel);

            var enquiry = new HostingEnquiry();

            enquiry.FirstName = viewModel.FirstName;
            enquiry.LastName = viewModel.LastName;
            enquiry.DiscordId = viewModel.DiscordId;
            enquiry.PackageType = viewModel.PackageType;

            _context.HostingEnquiries.Add(enquiry);
            _context.SaveChanges();

            return RedirectToAction("ContactSoon");
        }

        [Route("ContactSoon")]
        public IActionResult ContactSoon()
        {
            return View();
        }

        #endregion


    }
}
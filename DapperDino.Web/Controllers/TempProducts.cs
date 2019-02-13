using System;
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
    [Route("Products")]
    public class TempProductController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        #endregion

        #region Constructor(s)

        public TempProductController(ApplicationDbContext context, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #endregion

        #region Public Methods
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("RuneScapeBot")]
        public IActionResult ScrimBot()
        {
            var viewModel = new ProductsViewModel()
            {
                Product = "'RuneScape Gamble Bot'",
                PType = ProductEnquiryType.RuneScapeBot
            };

            return View("Contact", viewModel);
        }

        [Route("MusicBot")]
        public IActionResult MusicBot()
        {
            var viewModel = new ProductsViewModel()
            {
                Product = "'Music Bot'",
                PType = ProductEnquiryType.MusicBot
            };

            return View("Contact", viewModel);
        }

        [HttpPost("Contact")]
        public async Task<IActionResult> Post(ProductsViewModel viewModel)
        {
            if (!TryValidateModel(viewModel)) return RedirectToAction("Index", viewModel);

            var enquiry = new ProductEnquiry();

            enquiry.FirstName = viewModel.FirstName;
            enquiry.LastName = viewModel.LastName;
            enquiry.DiscordId = viewModel.DiscordId;
            enquiry.PType = viewModel.PType;

            _context.ProductEnquiries.Add(enquiry);
            _context.SaveChanges();

            await _hubContext.Clients.All.SendAsync("ProductEnquiry", viewModel);

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
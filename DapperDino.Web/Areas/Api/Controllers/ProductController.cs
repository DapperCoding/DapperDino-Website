using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Constants;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Areas.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DapperDino.Jobs;
using Microsoft.AspNetCore.SignalR;

namespace DapperDino.Areas.Api.Controllers
{
    [Route("/Api/Products")]
    public class ProductController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<DiscordBotHub> _hubContext;

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var products = _context.Products.Where(x=>x.IsActive).Include(x => x.Categories).ToArray();

            return Json(products);
        }

    }
}
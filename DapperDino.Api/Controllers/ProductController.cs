using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.Core.Mollie;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Jobs;
using DapperDino.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mollie.Api.Models;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly IPaymentStorageClient _paymentStorageClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<DiscordBotHub> _hubContext;
        #endregion

        #region Constructor(s)

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPaymentStorageClient paymentStorageClient, IHubContext<DiscordBotHub> hubContext)
        {
            _context = context;
            _paymentStorageClient = paymentStorageClient;
            _hubContext = hubContext;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(x => x.Categories).ThenInclude(x => x.ProductCategory)
                .Include(x => x.Instructions)
                .Include(x => x.ProductImages)
                .Where(x => x.IsActive)
                .ToList();


            return Json(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> ById(int productId)
        {
            var product = _context.Products
                .Include(x => x.Categories).ThenInclude(x => x.ProductCategory)
                .Include(x => x.Instructions)
                .Include(x => x.ProductImages)
                .SingleOrDefault(x => x.Id == productId);

            if (product == null) return NotFound("Product not found");

            return Json(product);
        }

        [HttpGet("Order/{id}/{discordId}")]
        [Authorize]
        public async Task<IActionResult> Order(int id, string discordId)
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var discordUser = await _context.DiscordUsers.SingleOrDefaultAsync(x => x.DiscordId == discordId);

            if (discordUser == null)
            {
                return BadRequest();
            }

            if (!await _userManager.IsInRoleAsync(user, RoleNames.Admin))
            {
                return BadRequest();
            }

            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var model = new CreatePaymentModel()
            {
                Amount = Decimal.Parse(product.Price.ToString()),
                Currency = Currency.USD,
                Description = product.Name + " - " + product.ShortDescription
            };

            var productAmounts = new List<ProductAmount>() { };

            productAmounts.Add(new ProductAmount()
            {
                ProductId = product.Id,
                Amount = 1
            });

            var order = new Order()
            {
                ProductAmounts = productAmounts,
                Status = OrderStatus.Open,
                UserId = user.Id
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var response = await _paymentStorageClient.Create(model, order.Id);
            order.MolliePaymentId = response.Id;

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("NewOrder", order);

            return Json(new { Url = response.Links.Checkout.Href});
        }
    }
}
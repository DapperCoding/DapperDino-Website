using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
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
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;

namespace DapperDino.Controllers
{
    // Faq controller
    [Route("Products")]
    public class ProductController : BaseControllerBase
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DiscordBotHub> _hubContext;
        private readonly IPaymentOverviewClient _paymentOverviewClient;
        private readonly IPaymentStorageClient _paymentStorageClient;
        private readonly IPaymentClient _paymentClient;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Constructor(s)

        public ProductController(
            ApplicationDbContext context,
            IHubContext<DiscordBotHub> hubContext,
            IPaymentOverviewClient paymentOverviewClient,
            IPaymentStorageClient paymentStorageClient,
            IPaymentClient paymentClient,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hubContext = hubContext;
            _paymentOverviewClient = paymentOverviewClient;
            _paymentStorageClient = paymentStorageClient;
            _paymentClient = paymentClient;
            _userManager = userManager;
        }

        #endregion

        #region Public Methods
        // Uses vue.js
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        // Aspnetcore 
        [Route("Information/{id}")]
        public async Task<IActionResult> Information(int id)
        {
            var product = await _context.Products.Include(x=>x.ProductImages).Include(x => x.Instructions).SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Route("Post")]
        [Authorize]
        public async Task<IActionResult> Post(Product posted)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == posted.Id);

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

            var response = await this._paymentStorageClient.Create(model, order.Id);
            order.MolliePaymentId = response.Id;

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("NewOrder", order);

            return Redirect(response.Links.Checkout.Href);
        }

        [Route("Download/{id:int}")]
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var orders = _context.Orders.Include(x => x.ProductAmounts).Where(x => x.ProductAmounts.Any(y => y.ProductId == id) && x.UserId == user.Id).ToArray();

            if (orders == null || orders.Length <= 0)
            {
                return RedirectToAction("Index");
            }
            foreach (var order in orders)
            {
                try
                {
                    var payment = await _paymentStorageClient.GetById(order.MolliePaymentId);

                    if (payment != null && payment.Status == Mollie.Api.Models.Payment.PaymentStatus.Paid)
                    {
                        return await Download($"{id}.zip", product.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction("Index");
        }

        private async Task<IActionResult> Download(string filename, string productName)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "Downloads/Products", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), $"{productName}.zip");
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".zip", MediaTypeNames.Application.Zip},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


        #endregion


    }
}
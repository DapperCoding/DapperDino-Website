using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Core.Mollie;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Services.Payment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mollie.Api.Client.Abstract;

namespace DapperDino.Areas.Client.Controllers
{
    [Route("/Client/Orders")]
    public class OrdersController : BaseController
    {
        #region Fields
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IPaymentStorageClient _paymentStorageClient;
        private IPaymentClient _paymentClient;
        #endregion

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPaymentStorageClient paymentStorageClient,
            IPaymentClient paymentClient)
        {
            _context = context;
            _userManager = userManager;
            _paymentStorageClient = paymentStorageClient;
            _paymentClient = paymentClient;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            var orders = _context.Orders.Where(x => x.UserId == user.Id).OrderByDescending(x=>x.CreationDate).ToArray();

            var ordersAndPayments = new List<OrderAndPaymentModel>();

            foreach (var order in orders)
            {
                try
                {
                    var payment = await _paymentStorageClient.GetById(order.MolliePaymentId);

                    if (payment == null)
                    {
                        continue;
                    }

                    var orderAndPayment = new OrderAndPaymentModel
                    {
                        Order = order,
                        Payment = payment
                    };

                    ordersAndPayments.Add(orderAndPayment);
                }
                catch (Exception ex)
                {

                    continue;
                }
                
            }

            return View(ordersAndPayments);
        }

        [Route("{orderId}")]
        public async Task<IActionResult> Order(int orderId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null )
            {
                return BadRequest();
            }

            var order = await _context.Orders.Include(x => x.ProductAmounts).ThenInclude(x => x.Product).SingleOrDefaultAsync(x => x.Id == orderId && x.UserId == user.Id);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            var payment = await _paymentStorageClient.GetById(order.MolliePaymentId);

            if (payment == null)
            {
                return RedirectToAction("Index");
            }

            var orderAndPayment = new OrderAndPaymentModel
            {
                Order = order,
                Payment = payment
            };

            return View(orderAndPayment);
        }
    }
}
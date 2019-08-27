using DapperDino.Areas.Admin.Models.Products;
using DapperDino.Core.Mollie;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.Services.Payment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mollie.Api.Client.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("Admin/Orders")]
    public class OrdersController:BaseController
    {

        #region Fields
        private ApplicationDbContext _context;
        private IPaymentStorageClient _paymentStorageClient;
        #endregion

        public OrdersController(ApplicationDbContext context, IPaymentStorageClient paymentStorageClient)
        {
            _context = context;
            _paymentStorageClient = paymentStorageClient;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {

            var orders = _context.Orders.OrderByDescending(x => x.CreationDate).ToArray();

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
            var order = await _context.Orders.Include(x => x.ProductAmounts).ThenInclude(x => x.Product).SingleOrDefaultAsync(x => x.Id == orderId);

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

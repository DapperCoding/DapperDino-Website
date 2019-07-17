using DapperDino.DAL.Models;
using Mollie.Api.Models.Payment.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Models.MollieModels
{
    public class OrderAndPaymentModel
    {
        public Order Order { get; set; }
        public PaymentResponse Payment { get; set; }
    }
}

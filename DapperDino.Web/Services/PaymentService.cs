using DapperDino.DAL;
using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Services
{
    public class PaymentService
    {
        private ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePaymentForProduct(Product product)
        {

        }




    }
}

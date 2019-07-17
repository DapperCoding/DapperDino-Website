using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models
{
    public class HomeViewModel
    {
        public int AmountOfProducts { get; set; } = 10;
        public int AmountOfActiveProducts { get; set; } = 10;
        public int AmountOfInactiveProducts { get; set; } = 10;

        public int AmountOfOrders { get; set; } = 100;
        public int AmountOfPaidOrders { get; set; } = 10;
        public double ShopIncome { get; set; } = 100.36;

        public int AmountOfHostingEnquiries { get; set; } = 10;

    }
}

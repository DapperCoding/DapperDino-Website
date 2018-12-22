using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL.Models;

namespace DapperDino.Areas.Admin.Models.Products
{
    public class IndexViewModel
    {
        public List<Product> Products { get; set; }
    }
}

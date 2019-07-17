using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models.Products
{
    public class ImagesOverviewViewModel
    {
        public int ProductId { get; set; }
        public List<ProductImage> Images { get; set; }
    }
}

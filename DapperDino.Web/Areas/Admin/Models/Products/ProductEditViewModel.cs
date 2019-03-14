using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models.Products
{
    public class ProductEditViewModel : Product
    {
        public List<ProductProductCategory> AllCategories { get; internal set; }
        public List<int> SelectedProductCategories { get; set; }
    }
}

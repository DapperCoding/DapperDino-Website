using DapperDino.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDino.Areas.Admin.Models.Products
{
    public class ProductCategoryEditViewModel:ProductCategory
    {
        public List<ProductCategory> AllCategories { get; set; }
    }
}

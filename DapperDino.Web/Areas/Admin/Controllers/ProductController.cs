using DapperDino.Areas.Admin.Models.Products;
using DapperDino.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("Admin/Product")]
    public class ProductController:BaseController
    {
        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        [Route("")]
        public IActionResult Index()
        {
            // Get list of all products
            var products = _context.Products.Include(x => x.Categories).Include(x=>x.Instructions).ToList();

            // Generate IndexViewModel using the products list
            var viewModel = new IndexViewModel()
            {
                
                Products = products
            };

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            // Get list of all products
            var product = _context.Products.Include(x => x.Categories).Include(x => x.Instructions).FirstOrDefault(x=>x.Id == id);

            // Does the product exist?
            if (product == null) return NotFound("Product can't be found");

            // Return the view -> using viewModel
            return View(product);
        }

        
    }
}
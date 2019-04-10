using DapperDino.Areas.Admin.Models.Products;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("Admin/Product")]
    public class ProductController : BaseController
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
            var products = _context.Products.Include(x => x.Categories).Include(x => x.Instructions).ToList();

            // Generate IndexViewModel using the products list
            var viewModel = new IndexViewModel()
            {

                Products = products
            };

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("Add")]
        public IActionResult Add()
        {
            // Get list of all products
            var viewModel = new ProductEditViewModel();

            viewModel.AllCategories = _context.ProductCategories
                .Select(x => new ProductProductCategory() { ProductCategory = x, ProductCategoryId = x.Id })
                .ToList();

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            // Get list of all products
            var product = _context.Products.Include(x => x.Categories).Include(x => x.Instructions).FirstOrDefault(x => x.Id == id);

            // Does the product exist?
            if (product == null) return NotFound("Product can't be found");

            var viewModel = new ProductEditViewModel();

            viewModel.Description = product.Description;
            viewModel.Id = product.Id;
            viewModel.ShortDescription = product.ShortDescription;
            viewModel.SalePercentage = product.SalePercentage;
            viewModel.Price = product.Price;
            viewModel.Categories = product.Categories;
            viewModel.Name = product.Name;

            viewModel.AllCategories = _context.ProductCategories
                .Select(x => new ProductProductCategory() { ProductCategory = x, ProductCategoryId = x.Id, Product = product, ProductId = product.Id })
                .ToList();

            // Return the view -> using viewModel
            return View(viewModel);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(ProductEditViewModel viewModel)
        {
            Product product;

            if (viewModel.Id > 0)
            {
                product = await _context.Products.Include(x=>x.Categories).SingleOrDefaultAsync(x=>x.Id == viewModel.Id);

                if (product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                product = new Product();
                _context.Products.Add(product);
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.ShortDescription = viewModel.ShortDescription;
            product.Price = viewModel.Price;
            product.SalePercentage = viewModel.SalePercentage;

            if (viewModel.Id <= 0)
            {
                await _context.SaveChangesAsync();
            }

            if (product.Categories == null )
            {
                product.Categories = new List<ProductProductCategory>();
            }


            if (viewModel.SelectedProductCategories != null)
            {
                if (product.Categories.Count > 0)
                    product.Categories.RemoveAll(x => true);

                foreach (var cat in viewModel.SelectedProductCategories)
                {
                    product.Categories.Add(new ProductProductCategory() { ProductId = product.Id, ProductCategoryId = cat });
                }

                await _context.SaveChangesAsync();
            }



            // Return the view -> using viewModel
            return RedirectToAction("Get", new { id = product.Id });
        }
    }
}
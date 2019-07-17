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
    [Route("Admin/ProductCategories")]
    public class OrderController:BaseController
    {

        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        [Route("")]
        public IActionResult Index()
        {
            var categories = _context.ProductCategories.ToList();

            return View(categories);
        }

        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var category = _context.ProductCategories.FirstOrDefault(x => x.Id == id);

            if (category == null) return NotFound("Product category not found");

            var viewModel = new ProductCategoryEditViewModel();

            viewModel.Categories = category.Categories;
            viewModel.Description = category.Description;
            viewModel.Id = category.Id;
            viewModel.Name = category.Name;
            viewModel.Parent = category.Parent;
            viewModel.ParentId = category.ParentId;

            viewModel.AllCategories = _context.ProductCategories.Where(x => x.Id != viewModel.Id).ToList();

            return View(viewModel);
        }

        [Route("Add")]
        public IActionResult Add(ProductCategoryEditViewModel viewModel = null)
        {
            if (viewModel == null) viewModel = new ProductCategoryEditViewModel();

            return View(viewModel);
        }

        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            // Get category from db
            var category = _context.ProductCategories.SingleOrDefault(x => x.Id == id);

            // If not found, return not found page
            if (category == null)
            {
                return NotFound();
            }

            // Remove from db
            _context.ProductCategories.Remove(category);

            // Save db
            _context.SaveChanges();

            // Return to overview
            return RedirectToAction("Index");
        }


        [HttpPost("Post")]
        public IActionResult Post(ProductCategoryEditViewModel category)
        {
            // Change to correct view
            // Validate category
            if (!ModelState.IsValid)
            {
                if (category.Id > 0)
                {
                    return RedirectToAction("Get", new { id = category.Id });
                }

                return RedirectToAction("Add", category);
            }

            // Update if id is present
            if (category.Id > 0)
            {
                // Update category
                var current = _context.ProductCategories.SingleOrDefault(x => x.Id == category.Id);

                // Shouldn't be possible
                if (current == null)
                    return RedirectToAction("Get", new { id = category.Id });

                // Set new values
                current.Name = category.Name;
                current.Description = category.Description;
            }
            else
            {
                // Add category
                _context.ProductCategories.Add(category);
            }

            // Save in db
            _context.SaveChanges();

            // Redirect to updated
            return RedirectToAction("Get", new { id = category.Id });
        }
    }
}

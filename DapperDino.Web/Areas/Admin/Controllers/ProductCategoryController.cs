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
    public class ProductCategoryController:BaseController
    {

        #region Fields

        // Shared context accessor
        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public ProductCategoryController(ApplicationDbContext context)
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

            return View(category);
        }

        [Route("Add")]
        public IActionResult Add()
        {
            var category = new ProductCategory();

            return View(category);
        }

        [Route("{id?}")]
        [HttpPost]
        public IActionResult Post(ProductCategory category, int? id = null)
        {
            if (!ModelState.IsValid) return View(category);

            if(id == null)
            {
                //add
                _context.Add(category);
                _context.SaveChanges();
            }
            else
            {
                //Edit
            }

            return RedirectToAction("Get", new { id = category.Id });
        }
    }
}

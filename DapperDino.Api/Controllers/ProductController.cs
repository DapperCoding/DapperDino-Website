using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : BaseController
    {
        #region Fields

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion

        #region Constructor(s)

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products
                .Include(x => x.Categories).ThenInclude(x=>x.ProductCategory)
                .Include(x => x.Instructions)
                .Include(x => x.ProductImages)
                .Where(x=>x.IsActive)
                .ToList();
            

            return Json(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> ById(int productId)
        {
            var product = _context.Products
                .Include(x => x.Categories).ThenInclude(x => x.ProductCategory)
                .Include(x => x.Instructions)
                .Include(x => x.ProductImages)
                .SingleOrDefault(x => x.Id == productId);

            if (product == null) return NotFound("Product not found");

            return Json(product);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Areas.Admin.Models.ProductInstructions;
using DapperDino.Controllers;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("/Admin/ProductInstructions")]
    public class ProductInstructionsController : BaseController
    {

        #region Fields
        private ApplicationDbContext _context;
        #endregion

        public ProductInstructionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("")]
        public IActionResult Index()
        {

            return View(_context.ProductInstructions.OrderByDescending(x => x.Id).Take(20).ToArray());
        }

        [Route("ForProduct/{id}")]
        public IActionResult ProductInstruction(int id)
        {
            var product = _context.Products.Include(x => x.Instructions).SingleOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productInstruction = product.Instructions != null ? new ProductInstructionsEditViewModel(product.Instructions) : new ProductInstructionsEditViewModel();

            if (productInstruction == null)
            {
                return NotFound();
            }

            productInstruction.ProductId = id;

            return View(productInstruction);
        }

        [HttpPost("Post")]
        public IActionResult ProductInstruction(ProductInstructionsEditViewModel viewModel)
        {
            var product = _context.Products.Include(x => x.Instructions).SingleOrDefault(x => x.Id == viewModel.ProductId);
            var productInstruction = new ProductInstructions();

            if (product == null)
            {
                return BadRequest();
            }

            if (product.ProductInstructionsId > 0 && viewModel.Id > 0 && product.ProductInstructionsId == viewModel.Id)
            {
                productInstruction = product.Instructions;
                productInstruction.Name = viewModel.Name;
                productInstruction.Url = viewModel.Url;
                productInstruction.Description = viewModel.Description;

                _context.SaveChanges();
            }
            else
            {

                productInstruction.Name = viewModel.Name;
                productInstruction.Url = viewModel.Url;
                productInstruction.Description = viewModel.Description;


                _context.ProductInstructions.Add(productInstruction);
                _context.SaveChanges();
            }
            product.Instructions = null;
            product.ProductInstructionsId = productInstruction.Id;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
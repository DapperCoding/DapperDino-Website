using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using DapperDino.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("/Admin/Proficiency")]
    public class ProficiencyController : BaseController
    {
        private ApplicationDbContext _context;
        private ProficiencyRepository _proficiencyRepository;

        public ProficiencyController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _proficiencyRepository = new ProficiencyRepository(_context);
        }

        [Route("")]
        public IActionResult Index()
        {
            var proficiencies = _proficiencyRepository.GetAll().Take(20);


            return View(proficiencies);
        }

        
        [Route("edit/{id?}")]
        public async Task<IActionResult> AddOrEdit(int? id)
        {

            Proficiency proficiency;

            if (id.HasValue)
            {
                proficiency = await _proficiencyRepository.GetById(id.Value);
            }
            else
            {
                proficiency = new Proficiency();
            }

            return View(proficiency);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> AddOrEditPost(Proficiency update)
        {

            if (update.Id > 0)
            {
                await _proficiencyRepository.Update(update.Id, update);
                ViewBag.Updated = true;
            }
            else
            {
                await _proficiencyRepository.Create(update);
            }

            _context.SaveChanges();

            return RedirectToAction("Edit", update.Id);
        }

        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) { return BadRequest(); }

            await _proficiencyRepository.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
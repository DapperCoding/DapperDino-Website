using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.Models.TicketViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DapperDino.Controllers
{
    // Create controller containing all IActionResults for the tickets
    public class TicketController : Controller
    {
        // create private readonly variable, this would be the container for the current ApplicationDbContext session
        private readonly ApplicationDbContext _dbContext;

        // create constructor that gets a new instance of ApplicationDbContext injected 
        public TicketController(ApplicationDbContext dbContext)
        {

            // set our own _dbContext to the injected ApplicationDbContext
            _dbContext = dbContext;
        }


        /// <summary>
        /// Create new IActionResult containing the equivalent of the API's /get
        /// This function contains all information to display a list of all the current tickets
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            // Create new instance of IndexViewModel class
            IndexViewModel viewModel = new IndexViewModel();

            // Get all tickets from database
            _dbContext.Tickets
                .Include(x=>x.Applicant)
                .Include(x=>x.AssignedTo);
            
            

            return View();
        }

        /// <summary>
        /// Create new IActionResult containing the equivalent of the API's /get
        /// This function contains all information to display a single ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IActionResult Index(int id)
        {
            return View();
        }
    }
}
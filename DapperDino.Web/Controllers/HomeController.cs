using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using DapperDino.Models;
using Microsoft.AspNetCore.Identity;

namespace DapperDino.Controllers
{
    public class HomeController : Controller
    {


        #region Fields

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructor(s)

        public HomeController(RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Create()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = _dbContext.ApplicationUsers.FirstOrDefault(a => a.Email == "rustenhovenmick@outlook.com"); 


                //Add default User to Role Admin    
                if (user != null)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            return Ok();
        }
    }
}

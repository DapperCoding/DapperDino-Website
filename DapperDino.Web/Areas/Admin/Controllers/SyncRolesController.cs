using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Admin.Controllers
{
    [Route("Admin/SyncRoles")]
    public class SyncRolesController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public SyncRolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ActionResult> Index()
        {
            await SyncRoles();

            return View();
        }

        private async Task SyncRoles()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

            }

            bool y = await _roleManager.RoleExistsAsync("HappyToHelp");
            if (!y)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "HappyToHelp";
                await _roleManager.CreateAsync(role);

            }
        }
    }
}
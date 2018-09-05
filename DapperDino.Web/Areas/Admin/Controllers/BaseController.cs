using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles=RoleNames.Admin)]
    public abstract class BaseController : Controller
    {

    }
}
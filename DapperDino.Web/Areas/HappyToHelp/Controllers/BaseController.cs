using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.Controllers;
using DapperDino.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.HappyToHelp.Controllers
{
    [Area("HappyToHelp")]
    [Authorize(Roles = "Administrator, HappyToHelp")]
    public abstract class BaseController : BaseControllerBase
    {

    }
}
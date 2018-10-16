using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperDino.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class BaseController:Controller
    {
    }
}

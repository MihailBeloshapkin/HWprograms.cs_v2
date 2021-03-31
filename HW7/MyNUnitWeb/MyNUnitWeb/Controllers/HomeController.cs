using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyNUnitWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
            => View("MyView");
    }
}

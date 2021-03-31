using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyNUnitWeb.Models;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyNUnitWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using HW5T1;
using System.Linq;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly CurrentStateModel currentState;
        private string pathToAssemblies;

        public HomeController(IWebHostEnvironment environment)
        {
            /*    if (!Directory.Exists($"{this.environment.WebRootPath}/Temp)"))
                {
                    Directory.CreateDirectory($"{this.environment.WebRootPath}/Temp");
                }
                this.environment = environment;
                this.pathToAssemblies = Path.Combine(this.environment.WebRootPath, "Assemblies");
                this.currentState = new CurrentStateModel(environment); */
            this.environment = environment;
            this.currentState = new CurrentStateModel(environment);
        }

        public IActionResult Index()
            => View("MyView", currentState);
        /*
        [HttpPost]
        public async Task<IActionResult> ExecuteTests()
        {
            var nunit = new HW5T1.MyNUnit($"{environment.WebRootPath}/Temp");
            nunit.Execute();
            var results = nunit.GetAllData();

        }*/

    }
}

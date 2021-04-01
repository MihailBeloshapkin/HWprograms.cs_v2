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
using System.Threading.Tasks;
using HW5T1;
using System.Linq;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly CurrentStateModel currentState;
        private AssemblyReportModel currentAssembly;
        private TestInfoContainer infoContainer;
        private string pathToAssemblies;

        public HomeController(IWebHostEnvironment environment)
        {
            this.environment = environment;
            /* if (!Directory.Exists($"{this.environment.WebRootPath}/Temp)"))
                 {
                     Directory.CreateDirectory($"{this.environment.WebRootPath}/Temp");
                 }
              /*   this.environment = environment;
                 this.pathToAssemblies = Path.Combine(this.environment.WebRootPath, "Assemblies");
                 this.currentState = new CurrentStateModel(environment); */
            this.currentAssembly = new AssemblyReportModel();
            this.infoContainer = new TestInfoContainer();
            this.currentState = new CurrentStateModel(environment);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index", infoContainer.TestReports);
        }

        [HttpPost]
        public IActionResult AddAssembly(IFormFile file)
        {
            if (file == null)
                return View(infoContainer.TestReports);

            using (var fileStream = new FileStream($"{environment.WebRootPath}/Temp/{file.FileName}", FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return RedirectToAction("Index", infoContainer.TestReports);
        }
        


        [HttpPost]
        public IActionResult ExecuteTests()
        {
            var nunit = new HW5T1.MyNUnit($"{environment.WebRootPath}/Temp");
            nunit.Execute();
            var results = nunit.GetAllData();
            var assemblyReport = new AssemblyReportModel();
            foreach (var test in results)
            {
                var currentReport = new TestReportModel();
                currentReport.Name = test.Name;
                currentReport.Time = test.TimeOfExecution;
                currentReport.WhyIgnored = test.WhyIgnored;
                if (test.Result == "Success")
                {
                    currentReport.Passed = true;
                    assemblyReport.Passed++;
                }
                else if (test.Result == "Failed")
                {
                    currentReport.Passed = false;
                    assemblyReport.Failed++;
                }
                else
                {
                    currentReport.Passed = null;
                    assemblyReport.Ignored++;
                }
                infoContainer.TestReports.Add(currentReport);
                assemblyReport.TestReports.Add(currentReport);
                
            }
            this.currentAssembly = assemblyReport;
            currentState.AssemblyReports.Add(assemblyReport);
            return View("Index", infoContainer.TestReports);
        }
    }
}

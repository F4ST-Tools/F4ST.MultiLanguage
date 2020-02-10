using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using F4ST.MultiLang;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ErrorViewModel = Test.Models.ErrorViewModel;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IResourcesProcessor _resourcesProcessor;

        public HomeController(ILogger<HomeController> logger,IResourcesProcessor resourcesProcessor)
        {
            _logger = logger;
            _resourcesProcessor = resourcesProcessor;
        }

        public IActionResult Index()
        { 

            var res=_resourcesProcessor.GetCultures();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

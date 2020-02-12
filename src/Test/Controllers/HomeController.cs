using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
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
        private readonly IJsonFileProcessor _processor;
        public HomeController(ILogger<HomeController> logger, IJsonFileProcessor processor)
        {
            _logger = logger;
            _processor = processor;
        }

        public IActionResult Index()
        {
            Thread.CurrentThread.CurrentCulture=new CultureInfo("fa");
            Thread.CurrentThread.CurrentUICulture=new CultureInfo("fa");
            var v = _processor.GetResource("Globals.Menu1");

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

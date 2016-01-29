using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace TechStackDemo.Controllers
{
    public class HomeController:Controller
    {
        private readonly ILogger logger;

        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
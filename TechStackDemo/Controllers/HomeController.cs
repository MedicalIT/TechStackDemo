using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Serilog;
using TechStackDemo.Hubs.Counter;
using TechStackDemo.Repository.Counter;

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

        public async Task<ActionResult> Index2()
        {

            var counterHubContext = GlobalHost.ConnectionManager.GetHubContext<ICounterHubClient>("CounterHub");
            await counterHubContext.Clients.All.NewCounterValue(new CounterItemDto(new CounterItem("a", 100)));

            return View("Index");
        }
    }
}
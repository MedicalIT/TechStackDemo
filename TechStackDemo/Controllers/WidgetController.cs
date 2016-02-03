using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;
using TechStackDemo.Repository.Counter;

namespace TechStackDemo.Controllers
{
    public class WidgetController:Controller
    {
        private readonly ICounterRepository counterRepository;
        private readonly ILogger logger;

        public WidgetController(ICounterRepository counterRepository, ILogger logger)
        {
            this.counterRepository = counterRepository;
            this.logger = logger;
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Castle.Windsor;
using TechStackDemo.Config;
using TechStackDemo.IoC;

[assembly: OwinStartup(typeof(TechStackDemo.Startup))]

namespace TechStackDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var logger = SerilogConfig.Configure();

            var container = WindsorConfig.Configure();
            var jsonSettings = JsonNetConfig.Configure(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);

            SignalRConfig.Configure(container, app, jsonSettings);
            WebApiConfig.Configure(container, jsonSettings);
            MvcConfig.Configure(container);
            


            WebApiConfig.EnsureConfigured();
        }
    }
}

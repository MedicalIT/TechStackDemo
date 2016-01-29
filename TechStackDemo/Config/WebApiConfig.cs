using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.Windsor;
using Newtonsoft.Json;
using TechStackDemo.IoC;
using TechStackDemo.Utils;

namespace TechStackDemo.Config
{
    public static class WebApiConfig
    {
        public static void Configure(IWindsorContainer container, JsonSerializerSettings jsonSettings)
        {
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WebApiControllerActivator(container)
                );

            //Map attribute routes firstly.  Use our inheriting custom direct route provider to allow for inheritance of routes (eg with the specific tag data controllers)
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes(new WebApiInheritingCustomDirectRouteProvider());


            //Regular WebAPI routing
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //JSON settings
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
        }

        public static void EnsureConfigured()
        {
            GlobalConfiguration.Configuration.EnsureInitialized();
        }
    }
}
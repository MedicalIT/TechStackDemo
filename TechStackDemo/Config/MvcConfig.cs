using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using TechStackDemo.IoC;

namespace TechStackDemo.Config
{
    public static class MvcConfig
    {
        public static void Configure(IWindsorContainer continer)
        {
            ControllerBuilder.Current.SetControllerFactory(new MvcControllerFactory(continer));

            //routes
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("favicon.ico");

            RouteTable.Routes.MapMvcAttributeRoutes();
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}
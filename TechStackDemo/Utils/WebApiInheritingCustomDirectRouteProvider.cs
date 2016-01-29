using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace TechStackDemo.Utils
{
    //As per http://www.asp.net/web-api/overview/releases/whats-new-in-aspnet-web-api-22#ARI
    //And http://stackoverflow.com/a/24969829/1053381
    public class WebApiInheritingCustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
        GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }
    }
}
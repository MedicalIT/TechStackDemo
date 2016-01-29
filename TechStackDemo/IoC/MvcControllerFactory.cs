using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.Windsor;

namespace TechStackDemo.IoC
{
    public class MvcControllerFactory:DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public MvcControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404,
                    $"The controller for path '{requestContext.HttpContext.Request.Path}' could not be found.");
            }

            return (IController) container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller);
        }
    }
}
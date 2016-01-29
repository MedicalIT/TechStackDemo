using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;
using static System.Reactive.Disposables.Disposable;

namespace TechStackDemo.IoC
{
    public class WebApiControllerActivator:IHttpControllerActivator
    {
        private readonly IWindsorContainer container;

        public WebApiControllerActivator(IWindsorContainer container)
        {
            this.container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (IHttpController) container.Resolve(controllerType);

            request.RegisterForDispose(Disposable.Create(() =>
            {
                this.container.Release(controller);
            }));

            return controller;
        }
    }
}
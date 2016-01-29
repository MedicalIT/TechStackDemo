using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace TechStackDemo.IoC
{
    public class SignalRDependencyResolver: DefaultDependencyResolver
    {
        private readonly IWindsorContainer container;

        public SignalRDependencyResolver(IWindsorContainer container)
        {
            this.container = container;
        }

        public override object GetService(Type serviceType)
        {
            if (container.Kernel.HasComponent(serviceType))
            {
                var resolved = container.Resolve(serviceType);

                
                //work around poor SignalR disposal - we release the hub here
                var asHub = resolved as IHub;
                if (asHub != null)
                {
                    container.Release(asHub);
                }

                return resolved;
            }
            return base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            if (this.container.Kernel.HasComponent(serviceType))
            {
                var containerResolved = container.ResolveAll(serviceType).Cast<object>().ToList();
                
                //work around poor SignalR disposal - release the hub(s) here
                foreach (var hub in containerResolved.OfType<IHub>())
                {
                    container.Release(hub);
                }

                return containerResolved;
            }

            return base.GetServices(serviceType);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;

namespace TechStackDemo.IoC
{
    public class SignalRInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //register hubs as transient.  Our SignalR resolver will work around poor support for disposal in SignalR
            container.Register(
                Classes.FromThisAssembly().BasedOn(typeof(Hub<>)).WithServiceSelf().WithServiceAllInterfaces().LifestyleTransient()
                );
        }
    }
}
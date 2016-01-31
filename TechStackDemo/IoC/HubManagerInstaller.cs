using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TechStackDemo.Hubs;

namespace TechStackDemo.IoC
{
    public class HubManagerInstaller: IWindsorInstaller
    {
        public HubManagerInstaller()
        {
            
        }
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn<HubManagerBase>().WithServiceAllInterfaces().WithServiceSelf().LifestyleSingleton()
                );
        }
    }
}
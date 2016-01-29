using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace TechStackDemo.IoC
{
    public class RepositoryInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .InNamespace("TechStackDemo.Repository", true)
                    .If(t=>t.Name.EndsWith("Repository"))
                    .WithServiceAllInterfaces()
                    .LifestyleSingleton()
                );
        }
    }
}
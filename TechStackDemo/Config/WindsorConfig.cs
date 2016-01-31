using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Castle.Facilities.Startable;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace TechStackDemo.Config
{
    public static class WindsorConfig
    {
        //public static IWindsorContainer Container;

        public static IWindsorContainer Configure(StartFlag startFlag)
        {

            var container = new WindsorContainer();
            container.AddFacility<StartableFacility>(sf => sf.DeferredStart(startFlag));
            container.AddFacility<TypedFactoryFacility>();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, allowEmptyCollections: false));
            container.Install(FromAssembly.This());

            //WindsorConfig.Container = container;

            return container;

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace TechStackDemo.Config
{
    public static class WindsorConfig
    {
        public static IWindsorContainer Configure()
        {

            var container = new WindsorContainer();
            container.Install(
                FromAssembly.This()
                );

            return container;

        }
    }
}
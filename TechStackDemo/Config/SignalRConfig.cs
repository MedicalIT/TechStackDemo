﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Transports;
using Newtonsoft.Json;
using Owin;
using TechStackDemo.IoC;

namespace TechStackDemo.Config
{
    public static class SignalRConfig
    {
        public static void Configure(IWindsorContainer container, IAppBuilder app, JsonSerializerSettings jsonSettings)
        {
            GlobalHost.DependencyResolver = new SignalRDependencyResolver(container);

            app
                .MapSignalR(new HubConfiguration()
                {
                    //Resolver = new SignalRDependencyResolver(container)
                });

            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = null;  //no limit
            GlobalHost.Configuration.DefaultMessageBufferSize = 500;


            //SignalR JSON.Net handling - register dependency
            var json = JsonSerializer.Create(jsonSettings);
            GlobalHost.DependencyResolver.Register(
                typeof(Newtonsoft.Json.JsonSerializer),
                () => json //returning same variable each time..  (singleton)
                );


            ////Disable websockets
            //var transportManager = GlobalHost.DependencyResolver.Resolve<ITransportManager>() as TransportManager;
            //transportManager.Remove("websockets");
        }
    }
}
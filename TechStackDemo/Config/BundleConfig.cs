using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TechStackDemo.Config
{
    public static class BundleConfig
    {
        public static void Configure()
        {
            BundleTable.EnableOptimizations = false;

            var bundles = BundleTable.Bundles;

            bundles.Clear();
            bundles.ResetAll();

            bundles.Add(new Bundle("~/bundles/site")
                .Include("~/Scripts/jquery/jquery-2.2.0.js")
                .Include("~/Scripts/Underscore/underscore.js")
                .Include("~/Scripts/common.js")
                .Include("~/Scripts/collections/collections.js")
                .Include("~/Scripts/knockout/knockout-3.4.0.js")
                .Include("~/Scripts/knockout/knockoutExtenders.js")
                .Include("~/Scripts/knockout/knockoutCollections.js")
                .Include("~/Scripts/knockout/Knockout.punches.js")
                .Include("~/Scripts/SignalR/jquery.signalR-2.2.0.js")
                );

            bundles.Add(new Bundle("~/bundles/RequireJS")
                .Include("~/Scripts/requireJS/require.js")
                .Include("~/Scripts/requireJS/requireJSConfig.js")
                //.Include("~/Scripts/requireJS/text.js")
                );

            bundles.Add(new Bundle("~/bundles/PostSignalRHubs")
                .Include("~/Scripts/SignalR/SignalRConfig.js")
                .Include("~/Scripts/repository.js")
                );

            bundles.Add(new Bundle("~/bundles/components")
                .IncludeDirectory("~/ClientSide", "*.js", true)
                );
            
        }
    }
}
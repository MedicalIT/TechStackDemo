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
            BundleTable.EnableOptimizations = true;

            var bundles = BundleTable.Bundles;

            bundles.Clear();
            bundles.ResetAll();

            
        }
    }
}
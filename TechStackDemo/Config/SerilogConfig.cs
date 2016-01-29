using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace TechStackDemo.Config
{
    public static class SerilogConfig
    {
        public static ILogger Configure()
        {
            var config = new Serilog.LoggerConfiguration();


            var logger = config.MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            Log.Logger = logger;
            return logger;
        }
    }
}
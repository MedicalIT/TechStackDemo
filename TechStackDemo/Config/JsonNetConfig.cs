using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace TechStackDemo.Config
{
    public static class JsonNetConfig
    {
        public static JsonSerializerSettings Configure(JsonSerializerSettings baseSettings)
        {
            var settings = baseSettings;

            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            
            //settings.ContractResolver

            settings.NullValueHandling = NullValueHandling.Include;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            JsonConvert.DefaultSettings = () => settings;

            return settings;
        }
    }
}
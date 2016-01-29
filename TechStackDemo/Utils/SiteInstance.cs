using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechStackDemo.Utils
{
    public static class SiteInstance
    {
        public static readonly string SITE_INSTANCE = Guid.NewGuid().ToString("N");
    }
}
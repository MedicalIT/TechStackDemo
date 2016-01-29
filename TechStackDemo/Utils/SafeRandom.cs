using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechStackDemo.Utils
{
    public static class SafeRandom
    {
        private static readonly Random random = new Random();
        private static readonly object l = new object();

        public static int Next(int max)
        {
            lock (l)
            {
                return random.Next(max);
            }
        }
    }
}
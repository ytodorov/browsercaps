using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Web.Infrastructure
{
    public static class WebXmlCache
    {
        private static string xml;
        public static string Xml
        {
            get
            {               
                return xml;
            }            
        }

        static WebXmlCache()
        {
            var appPath = HttpRuntime.AppDomainAppPath;
            var fullPath = Path.Combine(appPath, "libs", "System.Web.xml");
            xml = File.ReadAllText(fullPath);
        }
    }
}
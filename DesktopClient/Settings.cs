using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DesktopClient
{
    public static class Settings
    {
        //public static string baseUrl = System.Configuration.ConfigurationManager.AppSettings.Get("host_url");
            
        //   internal["host_url"].ConnectionString;
        public static string baseUrl = "http://localhost:64082";
    }
}

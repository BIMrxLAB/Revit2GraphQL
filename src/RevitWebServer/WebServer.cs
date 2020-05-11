using Autodesk.Revit.DB;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitWebServer
{
    public class WebServer
    {
        public static string Host { get; set; }
        public static string Port { get; set; }

        public static string BaseUrl = string.Format("http://{0}:{1}/", Host, Port);

        public static Document Doc;

        public WebServer(string _host, string _port, Document _doc)
        {
            Host = _host;
            Port = _port;
            BaseUrl = string.Format("http://{0}:{1}/", Host, Port);

            Doc = _doc;
        }
        public void Start()
        {
            WebApp.Start<Startup>(BaseUrl);
        }

    }
}

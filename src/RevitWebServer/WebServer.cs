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

        public static bool isBusy = false;

        public static Document Doc;

        private IDisposable _server = null;

        public WebServer(string _host, string _port, Document _doc)
        {
            Host = _host;
            Port = _port;
            BaseUrl = string.Format("http://{0}:{1}/", Host, Port);

            Doc = _doc;
        }
        public void Start()
        {
            _server = WebApp.Start<Startup>(BaseUrl);
        }

        public void Stop()
        {
            if(_server!=null)
            {
                _server.Dispose();
            }
        }
    }
}

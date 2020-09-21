using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
        public static UIDocument UIDoc;
        public static ExternalEvent exEvent;
        public static RevitTask aRevitTask;

        private IDisposable _server = null;

        public WebServer(string _host, string _port, Document _doc, UIDocument _uidoc, RevitTask _aRevitTask)
        {
            Host = _host;
            Port = _port;
            BaseUrl = string.Format("http://{0}:{1}/", Host, Port);

            Doc = _doc;
            UIDoc = _uidoc;
            aRevitTask = _aRevitTask;
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

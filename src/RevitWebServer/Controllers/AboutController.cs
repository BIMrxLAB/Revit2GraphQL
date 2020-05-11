using System.Collections.Generic;
using System.Web.Http;

namespace RevitWebServer.Controllers
{
    public class AboutController : ApiController
    {
        [HttpGet]
        public string About()
        {
            return WebServer.Doc.PathName;
        }

    }

}

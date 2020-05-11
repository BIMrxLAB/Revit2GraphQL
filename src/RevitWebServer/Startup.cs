using Owin;
using System.Linq;
using System.Web.Http;

namespace RevitWebServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            //https://braincadet.com/category/c-sharp/
            //app.Map("/api", builder =>
            //{
            //    HttpConfiguration config = new HttpConfiguration();
            //    config.MapHttpAttributeRoutes();
            //    app.UseWebApi(config);
            //});

            //http://www.learnonlineasp.net/2017/10/use-owin-to-self-host-aspnet-web-api.html
            var config = new HttpConfiguration();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { controller = "example", id = RouteParameter.Optional });

            app.UseWebApi(config);
        }

    }
}

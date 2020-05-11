using RevitGraphQLResolver;
using System.Threading.Tasks;
using System.Web.Http;

namespace RevitWebServer.Controllers
{
    [Route("graphql")]
    public class GraphqlController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] object query)
        {

            ResolverEntry aEntry = new ResolverEntry(WebServer.Doc);

            object result = await aEntry.GetResultAsync(query);

            return Ok(result);

        }

    }

}

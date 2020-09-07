using GraphQL;
using RevitGraphQLResolver;
using RevitGraphQLResolver.GraphQL;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace RevitWebServer.Controllers
{
    [Route("graphql")]
    public class GraphqlController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] GraphQLQuery query)
        {

            if (WebServer.isBusy)
            {
                var result = new GraphQLExecutionResult();
                result.errors.Add(new ExecutionError("Service is busy..."));

                return Ok(result);
            }
            else
            {
                WebServer.isBusy = true;

                ResolverEntry aEntry = new ResolverEntry(WebServer.Doc, WebServer.aRevitTask);

                GraphQLExecutionResult result = await aEntry.GetResultAsync(query);

                WebServer.isBusy = false;

                return Ok(result);
            }
        }

    }


}

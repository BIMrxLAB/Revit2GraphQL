using GraphQL;
using Newtonsoft.Json.Linq;
using RevitGraphQLResolver;
using System.Threading.Tasks;
using System.Web.Http;

namespace RevitWebServer.Controllers
{
    [Route("graphql")]
    public class GraphqlController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] JObject query)
        {
            if (WebServer.isBusy)
            {
                var result = new ExecutionResult();
                result.Errors = new ExecutionErrors();
                result.Errors.Add(new ExecutionError("Service is busy..."));

                return Ok(result);
            }
            else
            {
                WebServer.isBusy = true;

                ResolverEntry aEntry = new ResolverEntry(WebServer.Doc);

                object result = await aEntry.GetResultAsync(query);

                WebServer.isBusy = false;

                return Ok(result);
            }
        }

    }

    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }

    }


}

using GraphQL;
using RevitGraphQLResolver;
using RevitGraphQLResolver.GraphQL;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace RevitWebServer.Controllers
{
    [Route("graphql")]
    public class GraphqlController : ApiController
    {

        private readonly IDocumentWriter _writer;
        public GraphqlController()
        {
            _writer = new GraphQL.SystemTextJson.DocumentWriter(true);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage request, GraphQLQuery query)
        {

            var result = new ExecutionResult();
            if (WebServer.isBusy)
            {

                result.Errors.Add(new ExecutionError("Service is busy..."));

            }
            else
            {
                WebServer.isBusy = true;

                ResolverEntry aEntry = new ResolverEntry(WebServer.Doc, WebServer.aRevitTask);

                result = await aEntry.GetResultAsync(query);

                WebServer.isBusy = false;
            }

            var httpResult = result.Errors?.Count > 0
            ? HttpStatusCode.BadRequest
            : HttpStatusCode.OK;

            var json = await _writer.WriteToStringAsync(result);

            var response = request.CreateResponse(httpResult);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;

        }

    }


}

using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Instrumentation;
using Newtonsoft.Json.Linq;
using RevitGraphQLResolver.GraphQL;
using System;
using System.Threading.Tasks;

namespace RevitGraphQLResolver
{
    public class ResolverEntry
    {
        public static Document Doc { get; set; }
        public static RevitTask aRevitTask;

        public ResolverEntry(Document _doc, RevitTask _aRevitTask)
        {
            Doc = _doc;
            aRevitTask = _aRevitTask;
        }

        public async Task<object> GetResultAsync(JObject queryJObject)
        {
            var start = DateTime.UtcNow;

            GraphQLQuery query = queryJObject.ToObject<GraphQLQuery>();

            var inputs = query.Variables.ToInputs();

            var schema = new MySchema();

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema.GraphQLSchema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
                _.ExposeExceptions = true;
                _.EnableMetrics = true;
            });

            result.EnrichWithApolloTracing(start);

            return result;

        }
    }
}

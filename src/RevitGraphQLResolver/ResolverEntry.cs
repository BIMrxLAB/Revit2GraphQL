using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Instrumentation;
using Newtonsoft.Json.Linq;
using RevitGraphQLResolver.GraphQl;
using System;
using System.Threading.Tasks;

namespace RevitGraphQLResolver
{
    public class ResolverEntry
    {
        public static Document Doc { get; set; }

        public ResolverEntry(Document _doc)
        {
            Doc = _doc;
        }

        public async Task<object> GetResultAsync(object queryObject)
        {
            var start = DateTime.UtcNow;

            GraphQLQuery query = JObject.FromObject(queryObject).ToObject<GraphQLQuery>();

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

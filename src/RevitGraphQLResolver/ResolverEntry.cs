using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Instrumentation;
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

        public async Task<ExecutionResult> GetResultAsync(GraphQLQuery query)
        {
            var start = DateTime.UtcNow;

            var inputs = query.Variables.As<Inputs>();

            var schema = new MySchema();

            ExecutionResult result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema.GraphQLSchema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
                _.EnableMetrics = true;
            });

            result.EnrichWithApolloTracing(start);

            return result;

        }
    }
}

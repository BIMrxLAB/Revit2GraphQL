using System.Text.Json;

namespace RevitGraphQLResolver.GraphQL
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JsonElement Variables { get; set; }

    }
}

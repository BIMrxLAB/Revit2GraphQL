using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFabricationPart
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<QLParameter> qlParameters { get; set; }

    }
}

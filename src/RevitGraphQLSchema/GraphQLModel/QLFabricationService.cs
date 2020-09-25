using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFabricationService
    {
        public string id { get; set; }
        public string name { get; set; }

        public List<QLFabricationPart> qlFabricationParts { get; set; }
    }
}
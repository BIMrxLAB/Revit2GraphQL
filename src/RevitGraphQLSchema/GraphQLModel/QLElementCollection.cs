using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLElementCollection
    {
        public List<string> elementIds { get; set; }

        public List<QLFamilyInstance> qlFamilyInstances { get; set; }
        public List<QLFabricationPart> qlFabricationParts { get; set; }


    }
}

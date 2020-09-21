using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLAssembly
    {
        public string id { get; set; }
        public string name { get; set; }

        public bool hasViews { get; set; }

        public List<QLFamilyInstance> qlFamilyInstances { get; set; }
        public List<QLFabricationPart> qlFabricationParts { get; set; }


    }
}

using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFamilySymbol
    {
        public string id { get; set; }
        public string name { get; set; }

        public List<QLFamilyInstance> qlFamilyInstances { get; set; }

    }

}
using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFamilyCategory
    {
        public string name { get; set; }
        public List<QLFamily> qlFamilies { get; set; }

    }
}
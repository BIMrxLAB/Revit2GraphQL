using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFamily
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<QLFamilySymbol> qlFamilySymbols { get; set; }
    }
}
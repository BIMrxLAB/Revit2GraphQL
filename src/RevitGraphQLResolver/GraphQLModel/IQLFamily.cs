using System.Collections.Generic;

namespace RevitGraphQLResolver.GraphQLModel
{
    public interface IQLFamily
    {
        string id { get; set; }
        string name { get; set; }
        List<QLFamilySymbol> qlFamilySymbols { get; set; }
    }
}
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQLModel
{
    public interface IQLFamily
    {
        string id { get; set; }
        string name { get; set; }
        List<IQLFamilySymbol> qlFamilySymbols { get; set; }
    }
}
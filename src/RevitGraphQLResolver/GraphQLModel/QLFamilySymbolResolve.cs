using Autodesk.Revit.DB;
using RevitGraphQLSchema.GraphQLModel;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilySymbolResolve: QLFamilySymbol 
    {
        
        public QLFamilySymbolResolve(FamilySymbol _familySymbol)
        {
            id = _familySymbol.Id.ToString();
            name = _familySymbol.Name;
        }
    }
}

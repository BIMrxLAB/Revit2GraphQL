using Autodesk.Revit.DB;
using RevitGraphQLSchema.IGraphQLModel;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilySymbol:IQLFamilySymbol 
    {
        public string id { get; set; }
        public string name { get; set; }
        
        public QLFamilySymbol(FamilySymbol _familySymbol)
        {
            id = _familySymbol.Id.ToString();
            name = _familySymbol.Name;
        }
    }
}

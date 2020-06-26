using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLAssemblyResolve : QLAssembly 
    {

        public QLAssemblyResolve(AssemblyInstance _assembly, Field qlFieldViews, List<View> viewListing)
        {
            id = _assembly.Id.ToString();
            name = _assembly.Name;

            if (qlFieldViews != null && viewListing != null)
            {
                hasViews = viewListing.Any(v => v.AssociatedAssemblyInstanceId.IntegerValue == _assembly.Id.IntegerValue);
            }

        }


    }
}

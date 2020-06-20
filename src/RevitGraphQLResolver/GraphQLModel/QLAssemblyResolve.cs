using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using RevitGraphQLSchema.GraphQLModel;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLAssemblyResolve : QLAssembly 
    {

        public QLAssemblyResolve(AssemblyInstance _assembly)
        {
            id = _assembly.Id.ToString();
            name = _assembly.Name;

        }


    }
}

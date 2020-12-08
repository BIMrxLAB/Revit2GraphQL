using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLAssemblyResolve : QLAssembly
    {

        public QLAssemblyResolve(AssemblyInstance _assembly, List<View> viewListing, object aFieldOrContext)
        {
            id = _assembly.Id.ToString();
            name = _assembly.Name;

            var qlFieldViews = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "hasViews");
            if (qlFieldViews != null && viewListing != null)
            {
                hasViews = viewListing.Any(v => v.AssociatedAssemblyInstanceId.IntegerValue == _assembly.Id.IntegerValue);
            }

            var queryFieldForElementCollection = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "qlElementCollection");
            if (queryFieldForElementCollection != null)
            {
                //var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForEllementCollection, "qlFamilyInstances");
                //var qlFabricationPartsField = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForEllementCollection, "qlFabricationParts");
                ICollection<ElementId> elementIds = _assembly.GetMemberIds();
                //qlElementCollection = new QLElementCollectionResolve(elementIds, qlFamilyInstancesField, qlFabricationPartsField);
                qlElementCollection = new QLElementCollectionResolve(elementIds, queryFieldForElementCollection);
            }

        }

    }
}

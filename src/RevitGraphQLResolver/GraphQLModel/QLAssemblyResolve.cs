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

        public QLAssemblyResolve(AssemblyInstance _assembly, Field qlFieldViews, List<View> viewListing, Field queryFieldForFamilyInstances, Field queryFieldForFabricationParts)
        {
            id = _assembly.Id.ToString();
            name = _assembly.Name;

            if (qlFieldViews != null && viewListing != null)
            {
                hasViews = viewListing.Any(v => v.AssociatedAssemblyInstanceId.IntegerValue == _assembly.Id.IntegerValue);
            }

            if (queryFieldForFamilyInstances != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilyInstances, "nameFilter");
                var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilyInstances, "qlParameters");

                var elementIds = _assembly.GetMemberIds();
                var _doc = ResolverEntry.Doc;

                foreach (var aId in elementIds)
                {
                    var aElement = _doc.GetElement(aId);
                    if (aElement is FamilyInstance)
                    {
                        var x = aElement as FamilyInstance;

                        if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                        {
                            returnElementsObject.Add(new QLFamilyInstanceResolve(x, queryFieldForParameters));
                        }
                    }

                }

                qlFamilyInstances = returnElementsObject.OrderBy(x => x.name).ToList();
            }
            if (queryFieldForFabricationParts != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFabricationPart>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFabricationParts, "nameFilter");
                var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFabricationParts, "qlParameters");

                var elementIds = _assembly.GetMemberIds();
                var _doc = ResolverEntry.Doc;

                foreach (var aId in elementIds)
                {
                    var aElement = _doc.GetElement(aId);
                    if (aElement is FabricationPart)
                    {
                        var x = aElement as FabricationPart;

                        if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                        {
                            returnElementsObject.Add(new QLFabricationPartResolve(x, queryFieldForParameters));
                        }
                    }

                }

                qlFabricationParts = returnElementsObject.OrderBy(x => x.name).ToList();
            }

        }


    }
}

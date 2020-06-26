using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilySymbolResolve: QLFamilySymbol 
    {
        
        public QLFamilySymbolResolve(FamilySymbol _familySymbol, Field queryFieldForFamilyInstances)
        {
            id = _familySymbol.Id.ToString();
            name = _familySymbol.Name;

            if (queryFieldForFamilyInstances != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilyInstances, "nameFilter");
                var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilyInstances, "qlParameters");

                var _doc = ResolverEntry.Doc;
                List<FamilyInstance> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
                    .Select(x => (x as FamilyInstance)).Where(x => x.Symbol.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach (var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilyInstanceResolve(x, queryFieldForParameters));
                    }
                }

                qlFamilyInstances = returnElementsObject.OrderBy(x => x.name).ToList();
            }

        }
    }
}

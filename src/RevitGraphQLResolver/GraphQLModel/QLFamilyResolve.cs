using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilyResolve : QLFamily
    {

        public QLFamilyResolve()
        {

        }
        public QLFamilyResolve(Family _family, Field queryFieldForFamilySymbols)
        {
            id = _family.Id.ToString();
            name = _family.Name;

            if (queryFieldForFamilySymbols != null)
            {
                var returnElementsObject = new ConcurrentBag<QLFamilySymbol>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilySymbols, "nameFilter");

                var _doc = ResolverEntry.Doc;
                List<FamilySymbol> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilySymbol))
                    .Select(x => (x as FamilySymbol)).Where(x => x.Family.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach(var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilySymbolResolve(x));
                    }
                }

                qlFamilySymbols = returnElementsObject.OrderBy(x => x.name).ToList();

            }
        }
    }
}

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
        public QLFamilyResolve(Family _family, Field queryFieldForFamilySymbols, Field queryFieldForFamilyInstances)
        {
            id = _family.Id.ToString();
            name = _family.Name;

            if (queryFieldForFamilySymbols != null)
            {
                var returnElementsObject = new ConcurrentBag<QLFamilySymbol>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilySymbols, "nameFilter");
                var queryFieldForFamily2Instances = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilySymbols, "qlFamilyInstances");

                var _doc = ResolverEntry.Doc;
                List<FamilySymbol> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilySymbol))
                    .Select(x => (x as FamilySymbol)).Where(x => x.Family.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach(var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilySymbolResolve(x, queryFieldForFamily2Instances));
                    }
                }

                qlFamilySymbols = returnElementsObject.OrderBy(x => x.name).ToList();

            }

            if(queryFieldForFamilyInstances!=null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilyInstances, "nameFilter");
                var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilyInstances, "qlParameters");

                var _doc = ResolverEntry.Doc;
                List<FamilyInstance> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
                    .Select(x => (x as FamilyInstance)).Where(x => x.Symbol.Family.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach(var x in objectList)
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

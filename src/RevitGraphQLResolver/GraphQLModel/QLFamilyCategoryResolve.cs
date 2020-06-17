using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilyCategoryResolve : QLFamilyCategory
    {
        public QLFamilyCategoryResolve()
        {

        }

        public QLFamilyCategoryResolve(string qlFamilyCategoryName, Field queryFieldForFamilies)
        {
            name = qlFamilyCategoryName;

            if (queryFieldForFamilies != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamily>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilies, "nameFilter");
                var queryFieldForFamilySymbols = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilySymbols");

                var _doc = ResolverEntry.Doc;
                List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).Where(x => x.FamilyCategory.Name == name).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach(var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilyResolve(x, queryFieldForFamilySymbols));
                    }
                }



                qlFamilies = returnElementsObject.OrderBy(x => x.name).ToList();

            }
        }
    }
}

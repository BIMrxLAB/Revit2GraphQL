using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.IGraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilyCategory : IQLFamilyCategory
    {
        public string name { get; set; }

        public List<IQLFamily> qlFamilies { get; set; }

        public QLFamilyCategory()
        {

        }

        public QLFamilyCategory(string qlFamilyCategoryName, Field queryFieldForFamilies)
        {
            name = qlFamilyCategoryName;

            if (queryFieldForFamilies != null)
            {

                var returnElementsObject = new ConcurrentBag<IQLFamily>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilies, "nameFilter");
                var queryFieldForFamilySymbols = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilySymbols");

                var _doc = ResolverEntry.Doc;
                List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).Where(x => x.FamilyCategory.Name == name).ToList();

                Parallel.ForEach(objectList, x =>
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamily(x, queryFieldForFamilySymbols));
                    }
                });



                qlFamilies = returnElementsObject.OrderBy(x => x.name).ToList();

            }
        }
    }
}

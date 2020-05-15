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
    public class QLFamily : IQLFamily
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<IQLFamilySymbol> qlFamilySymbols { get; set; }

        public QLFamily()
        {

        }
        public QLFamily(Family _family, Field queryFieldForFamilySymbols)
        {
            id = _family.Id.ToString();
            name = _family.Name;

            if (queryFieldForFamilySymbols != null)
            {
                var returnElementsObject = new ConcurrentBag<IQLFamilySymbol>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilySymbols, "nameFilter");

                var _doc = ResolverEntry.Doc;
                List<FamilySymbol> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilySymbol))
                    .Select(x => (x as FamilySymbol)).Where(x => x.Family.Id.ToString() == id).ToList();

                Parallel.ForEach(objectList, x =>
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilySymbol(x));
                    }
                });

                qlFamilySymbols = returnElementsObject.OrderBy(x => x.name).ToList();

            }
        }
    }
}

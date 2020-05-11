using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevitGraphQLResolver.GraphQl
{
    public class Query : ObjectGraphType
    {


        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return ResolverEntry.Doc.PathName;
        }

        [GraphQLMetadata("sheets")]
        public List<string> GetSheets(ResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var sheetList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSheet))
                .Select(p => (ViewSheet)p).ToList();

            var stringList = sheetList.Select(x => x.Name).OrderBy(x => x).ToList();

            return stringList;
        }

        [GraphQLMetadata("families")]
        public List<string> GetFamilies(ResolveFieldContext context)
        {
            //Document _doc = context.UserContext as Document;
            Document _doc = ResolverEntry.Doc;

            var familyList = new FilteredElementCollector(_doc).OfClass(typeof(Family))
                .Select(p => (Family)p).ToList();

            var stringList = familyList.Select(x => x.Name).OrderBy(x=>x).ToList();

            return stringList;
        }

    }
}

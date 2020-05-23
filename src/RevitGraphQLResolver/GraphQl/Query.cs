using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Types;
using RevitGraphQLResolver.GraphQLModel;
using RevitGraphQLSchema.IGraphQl;
using RevitGraphQLSchema.IGraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevitGraphQLResolver.GraphQL
{
    public class Query : ObjectGraphType, IQuery
    {


        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return ResolverEntry.Doc.PathName;
        }

        [GraphQLMetadata("sheets")]
        public Task<List<string>> GetSheetsAsync(ResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var sheetList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSheet))
                .Select(p => (ViewSheet)p).ToList();

            var stringList = sheetList.Select(x => x.Name).OrderBy(x => x).ToList();

            return Task.FromResult(stringList);
        }

        //https://thebuildingcoder.typepad.com/blog/2012/05/the-schedule-api-and-access-to-schedule-data.html
        [GraphQLMetadata("schedules")]
        public Task<List<string>> GetSchedulesAsync(ResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var scheduleList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSchedule))
                .Select(p => (ViewSchedule)p).ToList();

            var stringList = scheduleList.Select(x => x.Name).OrderBy(x => x).ToList();

            return Task.FromResult(stringList);
        }

        [GraphQLMetadata("qlFamilies")]
        public Task<List<IQLFamily>> GetFamiliesAsync(ResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamilySymbolsField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilySymbols");

            var returnObject = new ConcurrentBag<IQLFamily>();

            //Parallel.ForEach(objectList, aFamily =>
            foreach(var aFamily in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aFamily.Name))
                {
                    var qlFamily = new QLFamily(aFamily, qlFamilySymbolsField);
                    returnObject.Add(qlFamily);
                }
            }
            return Task.FromResult(returnObject.OrderBy(x => x.name).ToList());

        }

        [GraphQLMetadata("qlFamilyCategories")]
        public Task<List<IQLFamilyCategory>> GetCategoriesAsync(ResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            var objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family).FamilyCategory);
            var stringList = objectList.Select(x => x.Name).OrderBy(x => x).Distinct().ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamiliesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilies");

            var returnObject = new ConcurrentBag<IQLFamilyCategory>();

            //Parallel.ForEach(stringList, aFamilyCategoryName =>
            foreach(var aFamilyCategoryName in stringList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aFamilyCategoryName))
                {
                    var qlFamilyCategory = new QLFamilyCategory(aFamilyCategoryName, qlFamiliesField);
                    returnObject.Add(qlFamilyCategory);
                }
            }
            return Task.FromResult(returnObject.OrderBy(x => x.name).ToList());

        }

    }
}

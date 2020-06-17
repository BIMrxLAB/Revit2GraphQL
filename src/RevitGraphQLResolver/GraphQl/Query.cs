using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Types;
using RevitGraphQLResolver.GraphQLModel;
using RevitGraphQLSchema.GraphQLModel;
using RevitGraphQLSchema.IGraphQl;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
        public List<string> GetSheets(ResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var sheetList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSheet))
                .Select(p => (ViewSheet)p).ToList();

            var stringList = sheetList.Select(x => x.Name).OrderBy(x => x).ToList();

            return stringList;
        }

        //https://thebuildingcoder.typepad.com/blog/2012/05/the-schedule-api-and-access-to-schedule-data.html
        [GraphQLMetadata("schedules")]
        public List<string> GetSchedules(ResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var scheduleList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSchedule))
                .Select(p => (ViewSchedule)p).ToList();

            var stringList = scheduleList.Select(x => x.Name).OrderBy(x => x).ToList();

            return stringList;
        }

        [GraphQLMetadata("qlFamilies")]
        public List<QLFamily> GetFamilies(ResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamilySymbolsField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilySymbols");

            var returnObject = new ConcurrentBag<QLFamily>();

            //Parallel.ForEach(objectList, aFamily =>
            foreach(var aFamily in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aFamily.Name))
                {
                    var qlFamily = new QLFamilyResolve(aFamily, qlFamilySymbolsField);
                    returnObject.Add(qlFamily);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }

        [GraphQLMetadata("qlFamilyCategories")]
        public List<QLFamilyCategory> GetCategories(ResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            var objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family).FamilyCategory);
            var stringList = objectList.Select(x => x.Name).OrderBy(x => x).Distinct().ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamiliesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilies");

            var returnObject = new ConcurrentBag<QLFamilyCategory>();

            //Parallel.ForEach(stringList, aFamilyCategoryName =>
            foreach(var aFamilyCategoryName in stringList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aFamilyCategoryName))
                {
                    var qlFamilyCategory = new QLFamilyCategoryResolve(aFamilyCategoryName, qlFamiliesField);
                    returnObject.Add(qlFamilyCategory);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }

    }
}

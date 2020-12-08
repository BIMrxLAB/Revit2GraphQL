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

        //public QLFamilyCategoryResolve(Category aCategory, Field queryFieldForFamilies, Field qlFamilyInstancesField)
        //{
        //    id = aCategory.Id.ToString();
        //    name = aCategory.Name;

        //    if (queryFieldForFamilies != null)
        //    {

        //        var returnElementsObject = new ConcurrentBag<QLFamily>();

        //        var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilies, "nameFilter");
        //        //var queryFieldForFamilySymbols = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilySymbols");
        //        //var queryFieldForFamily2Instances = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilyInstances");

        //        var _doc = ResolverEntry.Doc;
        //        List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).Where(x => x.FamilyCategory.Id.ToString() == id).ToList();

        //        //Parallel.ForEach(objectList, x =>
        //        foreach(var x in objectList)
        //        {
        //            if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
        //            {
        //                //returnElementsObject.Add(new QLFamilyResolve(x, queryFieldForFamilySymbols, queryFieldForFamily2Instances));
        //                returnElementsObject.Add(new QLFamilyResolve(x, queryFieldForFamilies));
        //            }
        //        }



        //        qlFamilies = returnElementsObject.OrderBy(x => x.name).ToList();

        //    }

        //    if (qlFamilyInstancesField != null)
        //    {

        //        var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

        //        var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(qlFamilyInstancesField, "nameFilter");
        //        //var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlParameters");

        //        var _doc = ResolverEntry.Doc;
        //        List<FamilyInstance> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
        //            .Select(x => (x as FamilyInstance)).Where(x => x.Symbol.Family.FamilyCategory.Id.ToString() == id).ToList();

        //        //Parallel.ForEach(objectList, x =>
        //        foreach (var x in objectList)
        //        {
        //            if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
        //            {
        //                returnElementsObject.Add(new QLFamilyInstanceResolve(x, queryFieldForFamilies));
        //            }
        //        }

        //        qlFamilyInstances = returnElementsObject.OrderBy(x => x.name).ToList();
        //    }

        //}
        public QLFamilyCategoryResolve(Category aCategory, object aFieldOrContext)
        {
            id = aCategory.Id.ToString();
            name = aCategory.Name;

            var queryFieldForFamilies = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "qlFamilies");
            if (queryFieldForFamilies != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamily>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilies, "nameFilter");
                //var queryFieldForFamilySymbols = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilySymbols");
                //var queryFieldForFamily2Instances = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlFamilyInstances");

                var _doc = ResolverEntry.Doc;
                List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).Where(x => x.FamilyCategory.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach(var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        //returnElementsObject.Add(new QLFamilyResolve(x, queryFieldForFamilySymbols, queryFieldForFamily2Instances));
                        returnElementsObject.Add(new QLFamilyResolve(x, queryFieldForFamilies));
                    }
                }



                qlFamilies = returnElementsObject.OrderBy(x => x.name).ToList();

            }

            var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "qlFamilyInstances");
            if (qlFamilyInstancesField != null)
            {

                var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(qlFamilyInstancesField, "nameFilter");
                //var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilies, "qlParameters");

                var _doc = ResolverEntry.Doc;
                List<FamilyInstance> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
                    .Select(x => (x as FamilyInstance)).Where(x => x.Symbol.Family.FamilyCategory.Id.ToString() == id).ToList();

                //Parallel.ForEach(objectList, x =>
                foreach (var x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilyInstanceResolve(x, queryFieldForFamilies));
                    }
                }

                qlFamilyInstances = returnElementsObject.OrderBy(x => x.name).ToList();
            }

        }
    }
}

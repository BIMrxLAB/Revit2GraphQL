using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using GraphQL;
using GraphQL.Types;
using RevitGraphQLResolver.GraphQLModel;
using RevitGraphQLSchema.GraphQLModel;
using RevitGraphQLSchema.IGraphQl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public List<string> GetSheets(IResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            var sheetList = new FilteredElementCollector(_doc)
                .OfClass(typeof(ViewSheet))
                .Select(p => (ViewSheet)p).ToList();

            var stringList = sheetList.Select(x => $"{x.Name} (ID: {x.Id})").OrderBy(x => x).ToList();

            return stringList;
        }

        [GraphQLMetadata("phases")]
        public List<string> GetPhases(IResolveFieldContext context)
        {
            Document _doc = ResolverEntry.Doc;

            List<string> returnObject = new List<string>();
            foreach(Phase aPhase in _doc.Phases)
            {
                returnObject.Add($"{aPhase.Name} (ID: {aPhase.Id})");
            }

            return returnObject.OrderBy(x => x).ToList();
        }

        [GraphQLMetadata("qlViewSchedules")]
        public List<QLViewSchedule> GetViewSchedules(IResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;
            var scheduleList = new FilteredElementCollector(_doc).OfClass(typeof(ViewSchedule)).Select(p => (ViewSchedule)p).Where(x=>!x.Name.Contains("Revision Schedule")).ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlViewScheduleData = GraphQlHelpers.GetFieldFromContext(context, "qlViewScheduleData");

            var returnObject = new ConcurrentBag<QLViewSchedule>();

            //Parallel.ForEach(objectList, aFamily =>
            foreach (var aViewSchedule in scheduleList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aViewSchedule.Name))
                {
                    var qlViewScheudle = new QLViewScheduleResolve(aViewSchedule, qlViewScheduleData);
                    returnObject.Add(qlViewScheudle);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();
        }

        [GraphQLMetadata("qlFamilies")]
        public List<QLFamily> GetFamilies(IResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            List<Family> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family)).ToList();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamilySymbolsField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilySymbols");
            var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilyInstances");

            var returnObject = new ConcurrentBag<QLFamily>();

            //Parallel.ForEach(objectList, aFamily =>
            foreach(var aFamily in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aFamily.Name))
                {
                    var qlFamily = new QLFamilyResolve(aFamily, qlFamilySymbolsField, qlFamilyInstancesField);
                    returnObject.Add(qlFamily);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }

        [GraphQLMetadata("qlSelectionFamilyInstances")]
        public List<QLFamilyInstance> GetSelection(IResolveFieldContext context, string[] nameFilter = null)
        {
            Document _doc = ResolverEntry.Doc;
            UIDocument _uidoc = ResolverEntry.UiDoc;

            Selection selection = _uidoc.Selection;
            ICollection<ElementId> selectedIds = selection.GetElementIds();

            var returnElementsObject = new ConcurrentBag<QLFamilyInstance>();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var queryFieldForParameters = GraphQlHelpers.GetFieldFromContext(context, "qlParameters");

            foreach (var aId in selectedIds)
            {
                var aElement = _doc.GetElement(aId);
                if(aElement is FamilyInstance)
                {
                    var x = aElement as FamilyInstance;
                    if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(x.Name))
                    {
                        returnElementsObject.Add(new QLFamilyInstanceResolve(x, queryFieldForParameters));
                    }
                }
            }

            List<QLFamilyInstance> qlFamilyInstances = returnElementsObject.OrderBy(x => x.name).ToList();

            return qlFamilyInstances;
        }

        [GraphQLMetadata("qlFamilyCategories")]
        public List<QLFamilyCategory> GetCategories(IResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            List<Category> objectList = new FilteredElementCollector(_doc).OfClass(typeof(Family)).Select(x => (x as Family).FamilyCategory).GroupBy(x => x.Id.ToString()).Select(x => x.First()).ToList();
            //var stringList = objectList.Select(x => x.Name).OrderBy(x => x).Distinct().ToList();
            //https://thebuildingcoder.typepad.com/blog/2010/05/categories.html
            //var objectList = _doc.Settings.Categories;

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();
            var qlFamiliesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilies");
            var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilyInstances");


            var returnObject = new ConcurrentBag<QLFamilyCategory>();

            //Parallel.ForEach(stringList, aFamilyCategoryName =>
            foreach(Category aCategory in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aCategory.Name))
                {
                    var qlFamilyCategory = new QLFamilyCategoryResolve(aCategory, qlFamiliesField, qlFamilyInstancesField);
                    returnObject.Add(qlFamilyCategory);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }

        [GraphQLMetadata("qlMepSystems")]
        public List<QLMepSystem> GetMepSystems(IResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            //https://github.com/jeremytammik/TraverseAllSystems/blob/master/TraverseAllSystems/Command.cs
            var objectList = new FilteredElementCollector(_doc).OfClass(typeof(MEPSystem)); //.Select(x=>x as MEPSystem).ToList<MEPSystem>();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();


            var returnObject = new ConcurrentBag<QLMepSystem>();

            //Parallel.ForEach(stringList, aFamilyCategoryName =>
            foreach (MEPSystem aMepSystem in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aMepSystem.Name))
                {
                    var qlMepSystem = new QLMepSystemResolve(aMepSystem);
                    returnObject.Add(qlMepSystem);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }

        [GraphQLMetadata("qlAssemblies")]
        public List<QLAssembly> GetAssemblies(IResolveFieldContext context, string[] nameFilter = null)
        {

            Document _doc = ResolverEntry.Doc;

            //https://github.com/jeremytammik/TraverseAllSystems/blob/master/TraverseAllSystems/Command.cs
            var objectList = new FilteredElementCollector(_doc).OfClass(typeof(AssemblyInstance)); //.Select(x=>x as MEPSystem).ToList<MEPSystem>();

            var nameFilterStrings = nameFilter != null ? nameFilter.ToList() : new List<string>();            
            var qlFieldViews = GraphQlHelpers.GetFieldFromContext(context, "hasViews");
            var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilyInstances");
            var qlFabricationPartsField = GraphQlHelpers.GetFieldFromContext(context, "qlFabricationParts");

            List<View> viewListing = null;
            if (qlFieldViews!=null)
            {
                viewListing = new FilteredElementCollector(ResolverEntry.Doc)
                             .OfCategory(BuiltInCategory.OST_Views)
                             .WhereElementIsNotElementType()
                             .Cast<View>().ToList();
            }

            var returnObject = new ConcurrentBag<QLAssembly>();

            //Parallel.ForEach(stringList, aFamilyCategoryName =>
            foreach (AssemblyInstance aAssembly in objectList)
            {
                if (nameFilterStrings.Count == 0 || nameFilterStrings.Contains(aAssembly.Name))
                {
                    var qlMepSystem = new QLAssemblyResolve(aAssembly, qlFieldViews, viewListing, qlFamilyInstancesField, qlFabricationPartsField);
                    returnObject.Add(qlMepSystem);
                }
            }
            return returnObject.OrderBy(x => x.name).ToList();

        }
    }
}

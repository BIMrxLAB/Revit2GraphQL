using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLElementCollectionResolve : QLElementCollection
    {

        //public QLElementCollectionResolve(ICollection<ElementId> _ids, Field queryFieldForFamilyInstances, Field queryFieldForFabricationParts)
        //{
        //    elementIds = _ids.Select(x=>x.ToString()).ToList();

        //    if (queryFieldForFamilyInstances != null || queryFieldForFabricationParts != null)
        //    {

        //        var _doc = ResolverEntry.Doc;
                
        //        var returnFamilyInstancesObject = new ConcurrentBag<QLFamilyInstance>();
        //        var returnFabricationPartsObject = new ConcurrentBag<QLFabricationPart>();


        //        foreach (var aId in _ids)
        //        {
        //            var aElement = _doc.GetElement(aId);
        //            if (aElement == null)
        //            {

        //            }
        //            else
        //            {
        //                if (aElement is FamilyInstance && queryFieldForFamilyInstances != null)
        //                {
        //                    var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilyInstances, "nameFilter");
        //                    var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilyInstances, "qlParameters");
        //                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(aElement.Name))
        //                    {
        //                        returnFamilyInstancesObject.Add(new QLFamilyInstanceResolve(aElement as FamilyInstance, queryFieldForParameters));
        //                    }
        //                }
        //                if (aElement is FabricationPart && queryFieldForFabricationParts != null)
        //                {
        //                    var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFabricationParts, "nameFilter");
        //                    var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFabricationParts, "qlParameters");
        //                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(aElement.Name))
        //                    {
        //                        returnFabricationPartsObject.Add(new QLFabricationPartResolve(aElement as FabricationPart, queryFieldForParameters));
        //                    }
        //                }
        //            }
        //        }
                
        //        qlFamilyInstances = returnFamilyInstancesObject.OrderBy(x => x.name).ToList();
        //        qlFabricationParts = returnFabricationPartsObject.OrderBy(x => x.name).ToList();
        //    }

        //}

        public QLElementCollectionResolve(ICollection<ElementId> _ids, object aFieldOrContext)
        {
            elementIds = _ids.Select(x=>x.ToString()).ToList();

            var queryFieldForFamilyInstances = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "qlFamilyInstances");
            var queryFieldForFabricationParts = GraphQlHelpers.GetFieldFromFieldOrContext(aFieldOrContext, "qlFabricationParts");


            if (queryFieldForFamilyInstances != null || queryFieldForFabricationParts != null)
            {

                var _doc = ResolverEntry.Doc;
                
                var returnFamilyInstancesObject = new ConcurrentBag<QLFamilyInstance>();
                var returnFabricationPartsObject = new ConcurrentBag<QLFabricationPart>();


                foreach (var aId in _ids)
                {
                    var aElement = _doc.GetElement(aId);
                    if (aElement == null)
                    {

                    }
                    else
                    {
                        if (aElement is FamilyInstance && queryFieldForFamilyInstances != null)
                        {
                            var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFamilyInstances, "nameFilter");
                            //var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFamilyInstances, "qlParameters");
                            if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(aElement.Name))
                            {
                                //returnFamilyInstancesObject.Add(new QLFamilyInstanceResolve(aElement as FamilyInstance, queryFieldForParameters));
                                returnFamilyInstancesObject.Add(new QLFamilyInstanceResolve(aElement as FamilyInstance, queryFieldForFamilyInstances));
                            }
                        }
                        if (aElement is FabricationPart && queryFieldForFabricationParts != null)
                        {
                            var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForFabricationParts, "nameFilter");
                            //var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(queryFieldForFabricationParts, "qlParameters");
                            if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(aElement.Name))
                            {
                                //returnFabricationPartsObject.Add(new QLFabricationPartResolve(aElement as FabricationPart, queryFieldForParameters));
                                returnFabricationPartsObject.Add(new QLFabricationPartResolve(aElement as FabricationPart, queryFieldForFabricationParts));
                            }
                        }
                    }
                }
                
                qlFamilyInstances = returnFamilyInstancesObject.OrderBy(x => x.name).ToList();
                qlFabricationParts = returnFabricationPartsObject.OrderBy(x => x.name).ToList();
            }

        }

    }
}

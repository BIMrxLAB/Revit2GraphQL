using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFabricationServiceResolve : QLFabricationService
    {

        public QLFabricationServiceResolve(FabricationService _fabService, Field qlFabricationPartsField)
        {

            id = _fabService.ServiceId.ToString();
            name = _fabService.Name;

            if(qlFabricationPartsField != null)
            {
                Autodesk.Revit.DB.Document _doc = ResolverEntry.Doc;

                List<FabricationPart> objectList = new FilteredElementCollector(_doc).OfClass(typeof(FabricationPart)).
                    Select(x => (x as FabricationPart)).Where(x=>x.ServiceId.ToString()==id).ToList();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(qlFabricationPartsField, "nameFilter");
                var queryFieldForParameters = GraphQlHelpers.GetFieldFromSelectionSet(qlFabricationPartsField, "qlParameters");

                qlFabricationParts = new List<QLFabricationPart>();
                foreach(FabricationPart aFabPart in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(aFabPart.Name))
                    {

                        qlFabricationParts.Add(new QLFabricationPartResolve(aFabPart, queryFieldForParameters));
                    }
                }
            }

        }


    }
}

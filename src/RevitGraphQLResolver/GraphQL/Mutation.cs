using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using GraphQL;
using GraphQL.Types;
using RevitGraphQLResolver.GraphQLModel;
using RevitGraphQLSchema.GraphQLModel;
using RevitGraphQLSchema.IGraphQl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitGraphQLResolver.GraphQL
{
    public class Mutation : ObjectGraphType, IMutation
    {
        [GraphQLMetadata("qlParameters")]
        public List<QLParameter> UpdateParameters(IResolveFieldContext context, List<UpdateQLParameter> input)
        {

            var _doc = ResolverEntry.Doc;
            var responseObject = new List<QLParameter>();

            ResolverEntry.aRevitTask.Run(app =>
            {

                // we need a transaction to modify the model
                using (Transaction trans = new Transaction(_doc, "Change global parameters values"))
                {
                    trans.Start();

                    foreach (var aUpdateParameter in input)
                    {
                        FamilyInstance aFamilyInstance = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
                            .Select(x => (x as FamilyInstance)).FirstOrDefault(x => x.Id.ToString() == aUpdateParameter.instanceId);
                        var aOne = _doc.GetElement(aUpdateParameter.instanceId);
                        var aTwo = _doc.GetElement(aUpdateParameter.parameterId);
                        if (aFamilyInstance != null)
                        {
                            foreach (Parameter aParameter in aFamilyInstance.Parameters)
                            {
                                if (aParameter.Id.ToString() == aUpdateParameter.parameterId)
                                {
                                    if (!aParameter.IsReadOnly)
                                    {
                                        bool setSuccess = false;
                                        switch (aParameter.StorageType)
                                        {
                                            case StorageType.None:
                                                break;
                                            case StorageType.Integer:
                                                setSuccess = aParameter.Set(int.Parse(aUpdateParameter.updateValue));
                                                break;
                                            case StorageType.Double:
                                                setSuccess = aParameter.Set(double.Parse(aUpdateParameter.updateValue));
                                                break;
                                            case StorageType.String:
                                                setSuccess = aParameter.Set(aUpdateParameter.updateValue);
                                                break;
                                            case StorageType.ElementId:
                                                setSuccess = aParameter.Set(new ElementId(int.Parse(aUpdateParameter.updateValue)));
                                                break;
                                            default:
                                                break;
                                        }
                                        if (setSuccess)
                                        {
                                            responseObject.Add(new QLParameter()
                                            {
                                                id = aParameter.Id.ToString(),
                                                name = aParameter.Definition.Name,
                                                value = aParameter.AsValueString() == null ? aParameter.AsString() : aParameter.AsValueString(),
                                                userModifiable = aParameter.UserModifiable
                                            });
                                        }
                                    }

                                }
                            }
                        }
                    }
                    trans.Commit();
                }

            });

            return responseObject;

        }

        [GraphQLMetadata("qlElementSelection")]
        public QLElementCollection SetSelection(IResolveFieldContext context, List<string> input)
        {
            Document _doc = ResolverEntry.Doc;
            UIDocument _uidoc = ResolverEntry.UiDoc;

            Selection selection = _uidoc.Selection;

            ICollection<ElementId> elementIds = selection.GetElementIds();
            elementIds.Clear();

            foreach(var aString in input)
            {
                elementIds.Add(new ElementId(int.Parse(aString)));
            }


            ResolverEntry.aRevitTask.Run(app =>
            {
                if (elementIds.Count > 0) selection.SetElementIds(elementIds);
            });

            //var qlFamilyInstancesField = GraphQlHelpers.GetFieldFromContext(context, "qlFamilyInstances");
            //var qlFabricationPartsField = GraphQlHelpers.GetFieldFromContext(context, "qlFabricationParts");

            QLElementCollection qlElementCollection = new QLElementCollectionResolve(elementIds, context);

            return qlElementCollection;
        }

    }
}

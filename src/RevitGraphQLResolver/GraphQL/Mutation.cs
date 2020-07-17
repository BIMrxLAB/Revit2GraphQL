using Autodesk.Revit.DB;
using GraphQL;
using GraphQL.Types;
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
        public async Task<List<QLParameter>> UpdateParametersAsync(List<UpdateQLParameter> input, ResolveFieldContext context)
        {

            var _doc = ResolverEntry.Doc;
            var responseObject = new List<QLParameter>();

            await ResolverEntry.aRevitTask.Run(app =>
            {

                // we need a transaction to modify the model
                using (Transaction trans = new Transaction(_doc, "Change global parameters values"))
                {
                    trans.Start();

                    foreach (var aUpdateParameter in input)
                    {
                        FamilyInstance aFamilyInstance = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance))
                            .Select(x => (x as FamilyInstance)).FirstOrDefault(x => x.Id.ToString() == aUpdateParameter.InstanceId);
                        var aOne = _doc.GetElement(aUpdateParameter.InstanceId);
                        var aTwo = _doc.GetElement(aUpdateParameter.ParameterId);
                        if (aFamilyInstance != null)
                        {
                            foreach (Parameter aParameter in aFamilyInstance.Parameters)
                            {
                                if (aParameter.Id.ToString() == aUpdateParameter.ParameterId)
                                {
                                    if (!aParameter.IsReadOnly)
                                    {
                                        bool setSuccess = false;
                                        switch (aParameter.StorageType)
                                        {
                                            case StorageType.None:
                                                break;
                                            case StorageType.Integer:
                                                setSuccess = aParameter.Set(int.Parse(aUpdateParameter.UpdateValue));
                                                break;
                                            case StorageType.Double:
                                                setSuccess = aParameter.Set(double.Parse(aUpdateParameter.UpdateValue));
                                                break;
                                            case StorageType.String:
                                                setSuccess = aParameter.Set(aUpdateParameter.UpdateValue);
                                                break;
                                            case StorageType.ElementId:
                                                setSuccess = aParameter.Set(new ElementId(int.Parse(aUpdateParameter.UpdateValue)));
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
    }
}

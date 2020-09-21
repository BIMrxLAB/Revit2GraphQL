using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFabricationPartResolve : QLFabricationPart
    {

        public QLFabricationPartResolve()
        {

        }
        public QLFabricationPartResolve(FabricationPart _fabricationPart, Field queryFieldForParameters)
        {
            id = _fabricationPart.Id.ToString();
            name = _fabricationPart.Name;

            if (queryFieldForParameters != null)
            {

                var returnElementsObject = new ConcurrentBag<QLParameter>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForParameters, "nameFilter");

                var _doc = ResolverEntry.Doc;
                ParameterSet objectList = _fabricationPart.Parameters;

                //Parallel.ForEach(objectList, x =>
                foreach (Parameter x in objectList)
                {
                    if (nameFiltersContained.Count == 0 || nameFiltersContained.Contains(x.Definition.Name))
                    {
                        returnElementsObject.Add(new QLParameter()
                        {
                            id = x.Id.ToString(),
                            name = x.Definition.Name,
                            value = x.AsValueString() == null ? x.AsString() : x.AsValueString(),
                            userModifiable = x.UserModifiable,
                            isReadOnly = x.IsReadOnly
                        });
                    }
                }

                qlParameters = returnElementsObject.OrderBy(x => x.name).ToList();
            }

        }
    }
}

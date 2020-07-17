using Autodesk.Revit.DB;
using GraphQL.Language.AST;
using RevitGraphQLResolver.GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLFamilyInstanceResolve : QLFamilyInstance
    {

        public QLFamilyInstanceResolve()
        {

        }
        public QLFamilyInstanceResolve(FamilyInstance _familyInstance, Field queryFieldForParameters)
        {
            id = _familyInstance.Id.ToString();
            name = _familyInstance.Name;

            if (queryFieldForParameters != null)
            {

                var returnElementsObject = new ConcurrentBag<QLParameter>();

                var nameFiltersContained = GraphQlHelpers.GetArgumentStrings(queryFieldForParameters, "nameFilter");

                var _doc = ResolverEntry.Doc;
                ParameterSet objectList = _familyInstance.Parameters;

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

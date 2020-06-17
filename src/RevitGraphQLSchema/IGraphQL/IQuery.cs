using GraphQL.Types;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IQuery
    {
        List<QLFamilyCategory> GetCategories(ResolveFieldContext context, string[] nameFilter = null);
        List<QLFamily> GetFamilies(ResolveFieldContext context, string[] nameFilter = null);
        string GetHello();
        List<string> GetSchedules(ResolveFieldContext context);
        List<string> GetSheets(ResolveFieldContext context);
    }
}
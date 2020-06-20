using GraphQL;
using GraphQL.Types;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IQuery
    {
        [GraphQLMetadata("qlFamilyCategories")]
        List<QLFamilyCategory> GetCategories(ResolveFieldContext context, string[] nameFilter = null);
        
        [GraphQLMetadata("qlFamilies")]
        List<QLFamily> GetFamilies(ResolveFieldContext context, string[] nameFilter = null);
        
        string GetHello();
        
        [GraphQLMetadata("qlViewSchedules")]
        List<QLViewSchedule> GetViewSchedules(ResolveFieldContext context, string[] nameFilter = null);
        
        [GraphQLMetadata("sheets")]
        List<string> GetSheets(ResolveFieldContext context);

        [GraphQLMetadata("qlMepSystems")]
        List<QLMepSystem> GetMepSystems(ResolveFieldContext context, string[] nameFilter = null);

        [GraphQLMetadata("qlAssemblies")]
        List<QLAssembly> GetAssemblies(ResolveFieldContext context, string[] nameFilter = null);
    }
}
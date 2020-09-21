using GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IQuery
    {
        [GraphQLMetadata("qlFamilyCategories")]
        List<QLFamilyCategory> GetCategories(IResolveFieldContext context, string[] nameFilter = null);
        
        [GraphQLMetadata("qlFamilies")]
        List<QLFamily> GetFamilies(IResolveFieldContext context, string[] nameFilter = null);
        
        string GetHello();
        
        [GraphQLMetadata("qlViewSchedules")]
        List<QLViewSchedule> GetViewSchedules(IResolveFieldContext context, string[] nameFilter = null);
        
        [GraphQLMetadata("sheets")]
        List<string> GetSheets(IResolveFieldContext context);

        [GraphQLMetadata("phases")]
        List<string> GetPhases(IResolveFieldContext context);

        [GraphQLMetadata("qlMepSystems")]
        List<QLMepSystem> GetMepSystems(IResolveFieldContext context, string[] nameFilter = null);

        [GraphQLMetadata("qlAssemblies")]
        List<QLAssembly> GetAssemblies(IResolveFieldContext context, string[] nameFilter = null);

        [GraphQLMetadata("qlSelectionFamilyInstances")]
        List<QLFamilyInstance> GetSelection(IResolveFieldContext context, string[] nameFilter = null);


    }
}
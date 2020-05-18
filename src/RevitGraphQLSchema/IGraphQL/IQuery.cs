using GraphQL.Types;
using RevitGraphQLSchema.IGraphQLModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IQuery
    {
        Task<List<IQLFamilyCategory>> GetCategoriesAsync(ResolveFieldContext context, string[] nameFilter = null);
        Task<List<IQLFamily>> GetFamiliesAsync(ResolveFieldContext context, string[] nameFilter = null);
        string GetHello();
        Task<List<string>> GetSchedulesAsync(ResolveFieldContext context);
        Task<List<string>> GetSheetsAsync(ResolveFieldContext context);
    }
}
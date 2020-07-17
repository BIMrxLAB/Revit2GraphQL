using GraphQL;
using GraphQL.Types;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IMutation
    {
        Task<List<QLParameter>> UpdateParametersAsync(List<UpdateQLParameter> input, ResolveFieldContext context);

    }
}
using GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IMutation
    {
        List<QLParameter> UpdateParameters(List<UpdateQLParameter> input, IResolveFieldContext context);

        QLElementCollection SetSelection(List<string> input, IResolveFieldContext context);
    }
}
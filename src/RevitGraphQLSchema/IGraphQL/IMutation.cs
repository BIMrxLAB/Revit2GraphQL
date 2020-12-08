using GraphQL;
using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQl
{
    public interface IMutation
    {
        List<QLParameter> UpdateParameters(IResolveFieldContext context, List<UpdateQLParameter> input);

        QLElementCollection SetSelection(IResolveFieldContext context, List<string> input);
    }
}
using GraphQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLResolver.GraphQL
{
    public class GraphQLExecutionResult
    {
        public object data { get; set; }
        public ExecutionErrors errors { get; set; }
        public GraphQLExecutionResult()
        {
            errors = new ExecutionErrors();
        }
        public GraphQLExecutionResult(object GraphQLData, ExecutionErrors GraphQLErrors)
        {
            data = GraphQLData;
            errors = GraphQLErrors;
        }
    }

}

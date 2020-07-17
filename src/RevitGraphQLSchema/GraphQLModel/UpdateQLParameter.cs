using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class UpdateQLParameter
    {
        public string InstanceId { get; set; }
        public string ParameterId { get; set; }
        public string UpdateValue { get; set; }
    }
}

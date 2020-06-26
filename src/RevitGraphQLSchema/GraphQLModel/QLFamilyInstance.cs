using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLFamilyInstance
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<QLParameter> qlParameters { get; set; }

    }
}

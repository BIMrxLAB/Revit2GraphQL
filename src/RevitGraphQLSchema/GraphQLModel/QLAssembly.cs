using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLAssembly
    {
        public string id { get; set; }
        public string name { get; set; }

        public bool hasViews { get; set; }
        public string levelName { get; set; }
        public string serviceName { get; set; }

    }
}

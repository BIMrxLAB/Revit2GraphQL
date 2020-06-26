using System;
using System.Collections.Generic;
using System.Text;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLTammTreeNode
    { 
        public string id { get; set; }
        public string text { get; set; }
        public List<QLTammTreeNode> children { get; set; }

    }
}

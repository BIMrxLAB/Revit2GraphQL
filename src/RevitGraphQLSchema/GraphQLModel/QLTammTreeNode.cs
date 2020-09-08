using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLTammTreeNode
    { 
        public string id { get; set; }
        public string text { get; set; }
        public List<QLTammTreeNode> children { get; set; }

    }
}

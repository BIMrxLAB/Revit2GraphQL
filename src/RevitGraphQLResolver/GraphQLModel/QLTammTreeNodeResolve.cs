using RevitGraphQLSchema.GraphQLModel;
using System.Collections.Generic;
using System.Text.Json;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLTammTreeNodeResolve : QLTammTreeNode
    {
        public QLTammTreeNodeResolve(JsonElement aJTreeNode)
        {
            id = aJTreeNode.GetProperty("id").GetString();
            text = aJTreeNode.GetProperty("text").GetString();
            children = new List<QLTammTreeNode>();
            foreach (JsonElement aJChild in (aJTreeNode.GetProperty("children").EnumerateArray()))
            {
                children.Add(new QLTammTreeNodeResolve(aJChild));
            }
        }
    }
}

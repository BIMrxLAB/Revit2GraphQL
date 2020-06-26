using Newtonsoft.Json.Linq;
using RevitGraphQLSchema.GraphQLModel;
using System;
using System.Collections.Generic;
using System.Text;
using TraverseAllSystems;

namespace RevitGraphQLResolver.GraphQLModel
{
    public class QLTammTreeNodeResolve : QLTammTreeNode
    {
        public QLTammTreeNodeResolve(JObject aJTreeNode)
        {
            id = aJTreeNode["id"].ToString();
            text = aJTreeNode["text"].ToString();
            children = new List<QLTammTreeNode>();
            foreach (JObject aJChild in (aJTreeNode["children"] as JArray))
            {
                children.Add(new QLTammTreeNodeResolve(aJChild));
            }
        }
    }
}

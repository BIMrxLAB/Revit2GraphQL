using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLParameter
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public bool userModifiable { get; set; }

        public bool isReadOnly { get; set; }

    }
}
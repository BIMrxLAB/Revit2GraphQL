using System.Collections.Generic;

namespace RevitGraphQLResolver.GraphQLModel
{
    public interface IQLFamilyCategory
    {
        string name { get; set; }
        List<QLFamily> qlFamilies { get; set; }
    }
}
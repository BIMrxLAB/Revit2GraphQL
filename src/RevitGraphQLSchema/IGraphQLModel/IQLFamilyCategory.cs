using System.Collections.Generic;

namespace RevitGraphQLSchema.IGraphQLModel
{
    public interface IQLFamilyCategory
    {
        string name { get; set; }
        List<IQLFamily> qlFamilies { get; set; }

    }
}
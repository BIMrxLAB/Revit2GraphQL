using System.Collections.Generic;

namespace RevitGraphQLSchema.GraphQLModel
{
    public class QLViewScheduleData
    {
        public List<string> headers { get; set; }

        public List<QLViewScheduleRow> rows { get; set; }


    }

}
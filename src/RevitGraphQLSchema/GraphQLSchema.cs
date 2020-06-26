namespace RevitGraphQLSchema
{
    public static class GraphQLSchema
    {
        public static readonly string schema = @"
            
            type QLFamilyCategory {
                name: String,
                qlFamilies(nameFilter: [String]): [QLFamily]
            }

            type QLFamily {
                id: String,
                name: String,
                qlFamilySymbols(nameFilter: [String]): [QLFamilySymbol]
            }

            type QLFamilySymbol {
                id: String,
                name: String
            }

            type QLViewSchedule {
                id: String,
                name: String,
                qlViewScheduleData: QLViewScheduleData
            }

            type QLViewScheduleData {
                headers: [String],
                rows: [QLViewScheduleRow]
            }

            type QLViewScheduleRow {
                id: String,
                cells: [String]
            }


            type QLMepSystem {
                id: String,
                name: String,
                mepDomain: String,
                qlTammTreeNode: QLTammTreeNode
            }

            type QLTammTreeNode {
                id: String,
                text: String,
                children: [QLTammTreeNode]
            }

            type QLAssembly {
                id: String,
                name: String,
                hasViews: Boolean
            }

            type Query {
                hello: String
            }

            type Query {
                sheets: [String]
            }

            type Query {
                qlMepSystems(nameFilter: [String]): [QLMepSystem]
            }

            type Query {
                qlAssemblies(nameFilter: [String]): [QLAssembly]
            }

            type Query {
                qlViewSchedules(nameFilter: [String]): [QLViewSchedule]
            }

            type Query {
                qlFamilyCategories(nameFilter: [String]): [QLFamilyCategory]
            }

            type Query {
                qlFamilies(nameFilter: [String]): [QLFamily]
            }

            ";

    }
}

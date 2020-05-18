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

            type Query {
                hello: String
            }

            type Query {
                sheets: [String]
            }

            type Query {
                schedules: [String]
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

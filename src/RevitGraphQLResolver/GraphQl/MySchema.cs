using GraphQL.Types;

namespace RevitGraphQLResolver.GraphQl
{
    public class MySchema
    {
        private ISchema _schema { get; set; }
        public ISchema GraphQLSchema
        {
            get
            {
                return this._schema;
            }
        }

        public MySchema()
        {
            this._schema = Schema.For(@"
            
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

            ", _ =>
                {
                    _.Types.Include<Query>();
                });
        }

    }

}

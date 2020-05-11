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
            
            type Query {
                hello: String
            }

            type Query {
                sheets: [String]
            }

            type Query {
                families: [String]
            }

            ", _ =>
                {
                    _.Types.Include<Query>();
                });
        }

    }

}

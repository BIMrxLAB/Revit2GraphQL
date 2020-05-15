using GraphQL.Types;

namespace RevitGraphQLResolver.GraphQL
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
            this._schema = Schema.For(RevitGraphQLSchema.GraphQLSchema.schema, _ =>
                {
                    _.Types.Include<Query>();
                });
        }

    }

}

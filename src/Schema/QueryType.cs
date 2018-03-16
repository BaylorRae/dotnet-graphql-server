using System;
using GraphQL.Types;

namespace Schema
{
    public class QueryType : ObjectGraphType
    {
        public QueryType()
        {
            Name = "Query";

            Field<NonNullGraphType<StringGraphType>>(
                name: "message",
                resolve: context => "Hello from GraphQL!"
            );
        }
    }
}
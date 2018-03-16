using GraphQL;

namespace Schema
{
    public class BicycleShopSchema : GraphQL.Types.Schema
    {
        public BicycleShopSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<QueryType>();
        }
    }
}
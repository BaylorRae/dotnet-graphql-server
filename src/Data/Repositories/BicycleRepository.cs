using System.Collections.Generic;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public interface IBicycleRepository : IRepository<Bicycle>
    {
        IEnumerable<Bicycle> FindByPartId(long partId);
    }
    
    public class BicycleRepository : Repository<Bicycle>, IBicycleRepository
    {
        public BicycleRepository(BicycleShopContext db) : base(db)
        { }

        public IEnumerable<Bicycle> FindByPartId(long partId)
        {
            return Set
                .Where(bicycle => bicycle.BicycleParts.Any(part => part.PartId == partId))
                .ToList();
        }
    }
}
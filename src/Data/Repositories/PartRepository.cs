using System.Collections.Generic;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public interface IPartRepository : IRepository<Part>
    {
        IEnumerable<Part> FindByBicycleId(long bicycleId);
    }
    
    public class PartRepository : Repository<Part>, IPartRepository
    {
        public PartRepository(BicycleShopContext db) : base(db)
        { }

        public IEnumerable<Part> FindByBicycleId(long bicycleId)
        {
            return Set
                .Where(part => part.BicycleParts.Any(bicycle => bicycle.BicycleId == bicycleId));
        }
    }
}
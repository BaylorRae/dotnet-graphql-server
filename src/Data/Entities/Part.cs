using System.Collections.Generic;
using Data.Types;

namespace Data.Entities
{
    public class Part : IProduct
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Discontinued { get; set; }

        public ICollection<BicyclePart> BicycleParts { get; set; } = new HashSet<BicyclePart>();
    }
}
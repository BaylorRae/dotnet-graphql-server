namespace Data.Entities
{
    public class BicyclePart
    {
        public long BicycleId { get; set; }
        public long PartId { get; set; }
        
        public Bicycle Bicycle { get; set; }
        public Part Part { get; set; }
    }
}
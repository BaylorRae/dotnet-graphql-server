namespace Data.Types
{
    public interface IProduct
    {
        long Id { get; set; }
        string Title { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
        bool Discontinued { get; set; }
    }
}
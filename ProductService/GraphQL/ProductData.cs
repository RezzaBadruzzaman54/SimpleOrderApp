namespace ProductService.GraphQL
{
    public class ProductData
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Stock { get; set; }
        public int Price { get; set; }
        public DateTime Created { get; set; }
    }
}

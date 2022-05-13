namespace OrderService.GraphQL
{
    public class OrderDataKafka
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

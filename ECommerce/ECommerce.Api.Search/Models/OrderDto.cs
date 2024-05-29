namespace ECommerce.Api.Search.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}

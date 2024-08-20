namespace Order.API.DTOs
{
    public class CreateOrderDto
    {
        public int BuyerId { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }
    }
}

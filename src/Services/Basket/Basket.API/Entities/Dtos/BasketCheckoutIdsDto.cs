namespace Basket.API.Entities.Dtos
{
    public class BasketCheckoutIdsDto
    {
        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

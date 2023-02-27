namespace Basket.API.Entities.Dtos
{
    public class ShoppingCartDto
    {
        public IEnumerable<ShoppintItemDto> ShoppingItems { get; set; }
        public string? Id { get; set; }
    }
}

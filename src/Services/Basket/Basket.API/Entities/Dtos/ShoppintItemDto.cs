namespace Basket.API.Entities.Dtos
{
    public class ShoppintItemDto
    {
        public string Id { get; set; }
        public string NameWithShortDesc { get; set; }
        public uint Qty { get; set; }
        public decimal Price { get; set; }
        public bool PiceUpdated { get; set; }
    }
}

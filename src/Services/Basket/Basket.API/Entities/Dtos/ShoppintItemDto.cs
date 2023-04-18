using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities.Dtos
{
    public class ShoppintItemDto
    {
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductNameShortdesc { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public uint Quantity { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter non a negative value")]
        public decimal UnitPrice { get; set; }
    }
}

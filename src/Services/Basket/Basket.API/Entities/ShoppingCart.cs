using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public Guid ShoppingCartId { get;  set; }
        public IEnumerable<ShoppingItem>? ShoppingItems { get; set; }
    }
}

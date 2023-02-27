using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Id { get; private set; }

        public IEnumerable<ShoppingItem> ShoppingItems { get; set; }
        public ShoppingCart()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}

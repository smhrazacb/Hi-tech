using Basket.API.Entities;
using Basket.API.Entities.Dtos;

namespace TestData
{
    public static class BasketData
    {
        public static ShoppingCart GetBasketData()
        {
            return new ShoppingCart()
            {
                UserId = "admin@admin.com",
                ShoppingItems = new List<ShoppingItem>()
                {
                    new ShoppingItem()
                    {
                    ProductId = "1",
                    PictureUrl = "",
                    ProductNameShortdesc = "Test",
                    Quantity = 5,
                    UnitPrice = 10
                    },
                     new ShoppingItem()
                    {
                    ProductId = "2",
                    PictureUrl = "",
                    ProductNameShortdesc = "Test2",
                    Quantity = 50,
                    UnitPrice = 100
                    }
                }
            };
        }
        // BasketCheckoutIdsDto dummy data
        public static BasketCheckoutIdsDto BasketCheckoutIdsDtoDummyData()
        {
            return new BasketCheckoutIdsDto()
            {
                UserId = "admin@admin.com",
                AddressLine = "Test",
                CardName = "Test",
                CardNumber = "Test",
                Country = "Test",
                CVV = "Test",
                EmailAddress = "Test",
                Expiration = "Test",
                FirstName = "Test",
                LastName = "Test",
                PaymentMethod = 1,
                State = "Test",
                TotalPrice = 100,
                ZipCode = "Test"
            };
        }
    }
}


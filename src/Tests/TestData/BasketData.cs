using Basket.API.Entities;
using Basket.API.Entities.Dtos;
using Catalog.API.Entities;

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
        public static ShoppingCart GetBasketData(IEnumerable<Category> categories)
        {
            return new ShoppingCart()
            {
                UserId = "admin@admin.com",
                ShoppingItems = new List<ShoppingItem>()
                {
                    new ShoppingItem()
                    {
                    ProductId = categories.FirstOrDefault().Id,
                    PictureUrl = categories.FirstOrDefault().SubCategory.Product.ImageUrl,
                    ProductNameShortdesc = categories.FirstOrDefault().SubCategory.Product.ProductNameShortdesc,
                    Quantity = (int)(categories.FirstOrDefault().SubCategory.Product.Quantity-2),
                    UnitPrice = categories.FirstOrDefault().SubCategory.Product.UnitPrice
                    }, new ShoppingItem()
                    {
                    ProductId = categories.LastOrDefault().Id,
                    PictureUrl = categories.LastOrDefault().SubCategory.Product.ImageUrl,
                    ProductNameShortdesc = categories.LastOrDefault().SubCategory.Product.ProductNameShortdesc,
                    Quantity = (int)(categories.LastOrDefault().SubCategory.Product.Quantity-2),
                    UnitPrice = categories.LastOrDefault().SubCategory.Product.UnitPrice
                    },
                    
                }
            };
        }
        // BasketCheckoutIdsDto dummy data
        public static BasketCheckoutIdsDto BasketCheckoutIdsDtoDummyData(string userId)
        {
            return new BasketCheckoutIdsDto()
            {
                UserId = userId,
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


using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {UserId =Guid.Parse("6832abf5-38e1-41d0-bab2-4f7aa13864a6"), FirstName = "Mehmet",
                    LastName = "Ozkaya", EmailAddress = "ezozkme@gmail.com",
                    State= "Sind",ZipCode="54565", AddressLine = "Bahcelievler",
                    Country = "Turkey", TotalPrice = 350,
                    CardName= "ABCCard Name", CardNumber= "545648-65546-45", CVV="555", Expiration="9-9-23", PaymentMethod= 1}
            };
        }
    }
}

using Customer.API.Entities;
using Customer.API.Entities.Enums;

namespace Customer.API.Data
{
    public class UserContextSeed
    {
        public static async Task SeedAsync(UserContext usercontext, ILogger<UserContextSeed> logger)
        {
            if (!usercontext.Users.Any())
            {
                usercontext.Users.AddRange(GetPreconfiguredUser());
                await usercontext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(UserContext).Name);
            }
        }
        public static IEnumerable<User> GetPreconfiguredUser()
        {
            var userlist = new List<User>();
            {
                for (int i = 0; i < 10; i++)
                {
                    userlist.Add(new User()
                    {
                        EmailAddress = Faker.Internet.Email(),
                        Password = Faker.Identification.UsPassportNumber(),
                        UserFullName = Faker.Name.FullName(Faker.NameFormats.WithPrefix),
                        OrderType = (EOrderType)Faker.RandomNumber.Next(0,1),
                        UserStatus = (EUserStatus)Faker.RandomNumber.Next(0, 2),
                        Address = new Address()
                        {
                            Country = Faker.Address.Country(),
                            State = Faker.Address.UsState(),
                            City = Faker.Address.City(),
                            NearByArea = Faker.Address.StreetName(),
                            HouseShopPlotNo = Faker.Address.SecondaryAddress(),
                            Addressline1 = Faker.Address.StreetAddress(),
                            Contact = new Contact() 
                            {
                                CountryCode =  Faker.Country.TwoLetterCode(), 
                                MobileNumber = Faker.Phone.Number(),
                                LandlineNumber = Faker.Phone.Number(),
                            },
                            GeoData = new GeoData() 
                            {
                                Latitude = Faker.RandomNumber.Next(-89,89), 
                                longitude = Faker.RandomNumber.Next(-180,180), 
                            }
                            
                        }
                    });
                }
                return userlist;
            }
        }
    }
}
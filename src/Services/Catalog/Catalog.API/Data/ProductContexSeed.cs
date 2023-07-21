using Catalog.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class ProductContextSeed
    {
        public static async void SeedData(IMongoCollection<Category> productCollection)
        {
            //if (productCollection.Find(p => true).Count() != GetPreconfiguredProducts().Count())
            //{
            var filter = new BsonDocument();
            await productCollection.DeleteManyAsync(filter);
            await productCollection.InsertManyAsync(GetPreconfiguredProducts());
            //}
        }
        public static IEnumerable<Category> GetPreconfiguredProducts()
        {
            return new List<Category>()
            {
                new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Diode",
                                AdditionalFields= new Dictionary<string, string>() {
                                    { "Color", "Red" } ,
                                    {"Mounting", "Surface Mount" }
                                },
                                Manufacturer= "ABC",
                                ManufacturerPartNo = "1n4001",
                                Packaging = "Per Piece",
                                UnitPrice = 200,
                                Series = "Automotive",
                                Quantity = 9
                            }
                        }
                },
                new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Diode",
                                AdditionalFields= new Dictionary<string, string>() {
                                    { "Color", "Red" } ,
                                    {"Mounting", "Surface Mount" }
                                },
                                Manufacturer= "ABC",
                                ManufacturerPartNo = "1n4001",
                                Packaging = "Per Piece",
                                UnitPrice = 200,
                                Series = "Industrial",
                                Quantity = 9
                            }
                        }
                },
                new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Diode",
                                AdditionalFields= new Dictionary<string, string>() {
                                    { "Color", "Red" } ,
                                    {"Mounting", "Surface Mount" }
                                },
                                Manufacturer= "ABC",
                                ManufacturerPartNo = "1n4001",
                                Packaging = "Per Piece",
                                UnitPrice = 200,
                                Series = "Automotive",
                                Quantity = 9
                            }
                        }
                },
                new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Transistor",
                                AdditionalFields = new Dictionary<string, string>() {
                                    { "Color", "Blue" },
                                    {"Mounting", "Surface Mount" },
                                    { "Operating Temperature", "45 celcius"}
                                },
                                Manufacturer = "ABC",
                                ManufacturerPartNo = "tn4001",
                                Packaging = "Per Piece",
                                UnitPrice = 100,
                                Series = "Automotive",
                                Quantity = 50
                            }
                        }
                },new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Transistor",
                                AdditionalFields = new Dictionary<string, string>() {
                                    { "Color", "Blue" },
                                    {"Mounting", "Surface Mount" },
                                    { "Operating Temperature", "45 celcius"}
                                },
                                Manufacturer = "ABC",
                                ManufacturerPartNo = "tn4001",
                                Packaging = "Per Piece",
                                UnitPrice = 100,
                                Series = "Automotive",
                                Quantity = 50
                            }
                        }
                },new Category()
                {
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Transistor",
                                AdditionalFields = new Dictionary<string, string>() {
                                    { "Color", "Blue" },
                                    {"Mounting", "Surface Mount" },
                                    { "Operating Temperature", "45 celcius"}
                                },
                                Manufacturer = "ABC",
                                ManufacturerPartNo = "tn4001",
                                Packaging = "Per Piece",
                                UnitPrice = 100,
                                Series = "Automotive",
                                Quantity = 50
                            }
                        }
                },
                 new Category()
                 {
                     CategoryName = "Mechanical",
                     SubCategory = new SubCategory()
                        {
                         SubCategoryName = "Hardware",
                         Product = new Product()
                         {
                             Name = "Hinge",
                             AdditionalFields= new Dictionary<string, string>() { { "Color", "Red" } },
                             Manufacturer= "ABghC",
                             ManufacturerPartNo = "fgdf4",
                             Packaging = "Per Piece",
                             UnitPrice = 2030,
                             Series = "Automotive",
                             Quantity = 9
                         },
                     }
                 },
                new Category()
                {
                    CategoryName = "Mechanical",
                    SubCategory = new SubCategory()
                    {
                        SubCategoryName = "Tools",
                        Product = new Product()
                        {
                            Name = "Drill Bit",
                            AdditionalFields = new Dictionary<string, string>() { { "Body", "Solid" } },
                            Manufacturer = "ABC",
                            ManufacturerPartNo = "tn4fgf001",
                            Packaging = "Per Piece",
                            UnitPrice = 16400,
                            Series = "Automotive",
                            Quantity = 560
                        },
                    }
                },
                new Category()
                {
                    CategoryName = "Mechanical",
                    SubCategory = new SubCategory()
                    {
                        SubCategoryName = "Bracket",
                        Product = new Product()
                        {
                            Name = "Right Angle Bracket",
                            AdditionalFields= new Dictionary<string, string>() { { "Color", "Red" } },
                            Manufacturer= "AdfBffC",
                            ManufacturerPartNo = "gdfdfg45345",
                            Packaging = "Per Piece",
                            UnitPrice = 900,
                            Series = "Automotive",
                            Quantity = 4
                        },
                    }
                }
            };
        }
    }
}

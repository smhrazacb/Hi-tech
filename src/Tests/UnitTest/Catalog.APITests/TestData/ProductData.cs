using Amazon.SecurityToken.Model.Internal.MarshallTransformations;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.APITests.TestData
{
    public static class ProductData
    {
        public static IEnumerable<CategoryWithCount> GetTestProductsCount()
        {
            // genertate test data list of CategoryWithCount
            var products = new List<CategoryWithCount>
            {
                new CategoryWithCount {CategoryName="category1", SubCategoryName="subcategory1", SubCategoryCount=1},
                new CategoryWithCount {CategoryName="category2", SubCategoryName="subcategory2", SubCategoryCount=2},
                new CategoryWithCount {CategoryName="category3", SubCategoryName="subcategory3", SubCategoryCount=3},
                new CategoryWithCount {CategoryName="category4", SubCategoryName="subcategory4", SubCategoryCount=4},
            };
            return products;
        }
        // gererate test data for new Entities.Category()        // gererate test data for new Entities.Category()
        public static IEnumerable<Category> GetPreconfiguredProducts()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = "1",
                    CategoryName = "Mechanical",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Tools",
                            Product =
                                new Product()
                            {
                                Name = "Diode",
                                AdditionalFields= new Dictionary<string, string>() { { "Color", "Red" } },
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
                    Id = "2",
                    CategoryName = "Electronics",
                    SubCategory =
                        new SubCategory()
                        {
                            SubCategoryName = "Component",
                            Product =
                                new Product()
                            {
                                Name = "Transistor",
                                AdditionalFields = new Dictionary<string, string>() { { "Color", "Blue" } },
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
                     Id = "2",
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
                    Id = "3",
                    CategoryName = "Mechanical",
                    SubCategory = new SubCategory()
                    {
                        SubCategoryName = "Tools",
                        Product = new Product()
                        {
                            Name = "Drill Bit",
                            AdditionalFields = new Dictionary<string, string>() { { "Color", "Blue" } },
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
                    Id = "4",
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
                },
            };
        }
    }
}

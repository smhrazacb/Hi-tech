using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Microsoft.AspNetCore.Http;

namespace TestData.TestData
{
    public static class ProductData
    {

        public static IFormFile GetFile() 
        {
            //Setup mock file using a memory stream
            var content =
                "Category,SubCategory,ManufacturerPartNo,Manufacturer,ItemName,Description,Stock,Price,Packaging,Series,DatasheetUrl,ImageUrls,AdditionalFields\r\nPassive Components,EMI / RFI Components,BLM18AG601SN1D,Murata Electronics,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0603 1LN,44,28.005,Tape & Reel (TR),\"EMIFIL®, BLM18\",https://www.murata.com/en-us/products/productdata/8796738650142/ENFA0003.pdf,https://media.digikey.com/Renders/Murata%20Renders/0603(LQG18).jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\nPassive Components,EMI / RFI Components,MMZ0603S601CT000,TDK Corporation,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0201 1LN,8,33.606,Tape & Reel (TR),MMZ,https://product.tdk.com/en/system/files?file=dam/doc/product/emc/emc/beads/catalog/beads_commercial_signal_mmz0603_en.pdf,https://media.digikey.com/Renders/TDK%20Renders/MMZ0603xxxxCT000.jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\n";
            var fileName = "test.csv";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            //create FormFile with desired data
            return new FormFile(stream, 0, stream.Length, "id_from_form", fileName);
        }
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
                     Id = "3",
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
                    Id = "4",
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
                    Id = "5",
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

        public static IEnumerable<Category> CategoryData(int index)
        {
            return new List<Category>() { GetPreconfiguredProducts().ElementAt(index) };
        }
    }
}

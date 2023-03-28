using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Validations;
using LumenWorks.Framework.IO.Csv;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using Sylvan.Data.Csv;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Catalog.API.Utilities
{


    public class CSV2Category
    {
        public CSVDto Read(string path)
        {
            CSVDto cSVDto = new CSVDto();
            cSVDto.NewCategories = new List<Category>();
            cSVDto.InvalidEntries = new List<string>();
            cSVDto.DuplicatePartNumbers = new List<string>();
            cSVDto.UpdateCategories = new List<Category>();
            var stopWatch = Stopwatch.StartNew();
            int count = 0;
            using (var reader = CsvDataReader.Create(path))
            {
                // Loop through each row in the CSV file
                while (reader.Read())
                {
                    try
                    {
                        if (cSVDto.NewCategories.Count == 28)
                        {
                            Console.WriteLine();
                        }
                        // Create a new instance of your custom class
                        Category obj = new Category();

                        // Set the properties of the product object from the data in the CSV file
                        obj.CategoryName = reader.GetString(0);

                        obj.SubCategory = new SubCategory();

                        obj.SubCategory.SubCategoryName = reader.GetString(1);
                        obj.SubCategory.Product = new Product();

                        obj.SubCategory.Product.ManufacturerPartNo = reader.GetString(2);
                        obj.SubCategory.Product.Manufacturer = reader.GetString(3);
                        obj.SubCategory.Product.Name = reader.GetString(4);
                        obj.SubCategory.Product.Description = reader.GetString(5);
                        int intValue = reader.GetInt32(6);
                        // converting Int32 to Uint32                    
                        uint uintValue = unchecked((uint)intValue);

                        obj.SubCategory.Product.Stock = uintValue;
                        obj.SubCategory.Product.Price = reader.GetDecimal(7);
                        obj.SubCategory.Product.Packaging = reader.GetString(8);
                        obj.SubCategory.Product.Series = reader.GetString(9);
                        obj.SubCategory.Product.DataSheetUrl = reader.GetString(10);
                        obj.SubCategory.Product.Image = reader.GetString(11);

                        string input = reader.GetString(12);
                        if (!input.IsNullOrEmpty())
                        {
                            // remove curly braces
                            input = input.Replace("[", "").Replace("]", "");

                            // remove curly braces
                            input = input.Replace("{", "").Replace("}", "");

                            // split key-value pairs using comma delimiter
                            string[] pairs = input.Split(',');
                            obj.SubCategory.Product.AdditionalFields =
                                new Dictionary<string, string>();
                            for (int i = 0; i < pairs.Length; i++)
                            {
                                obj.SubCategory.Product.AdditionalFields
                                    .Add(pairs[i], pairs[++i]);
                            }
                        }
                        if (cSVDto.NewCategories
                            .Where(a => a.SubCategory.Product.ManufacturerPartNo ==
                                obj.SubCategory.Product.ManufacturerPartNo &&
                                a.SubCategory.Product.Manufacturer ==
                                obj.SubCategory.Product.Manufacturer).Count() > 0)
                            cSVDto.DuplicatePartNumbers.Add(reader.GetRawRecordSpan().ToString());
                        else
                            cSVDto.NewCategories.Add(obj);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        cSVDto.InvalidEntries.Add(reader.GetRawRecordSpan().ToString());
                    }
                }
                //Console.WriteLine(count);
                Console.WriteLine($"seconds : {stopWatch.Elapsed.TotalSeconds}");
                Console.WriteLine($"Total Products added : {cSVDto.NewCategories.Count}");
                return cSVDto;
            }
        }
    }
}

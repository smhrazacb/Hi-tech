using Catalog.API.Data;
using Catalog.API.Entities.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServicesTest.Extensions;
using ServicesTest.Services;
using static MassTransit.ValidationResultExtensions;

namespace ServicesTest.TestCases
{
    public class Scenario1
    {
        private readonly CatalogService _fixture;
        string productId;
        public Scenario1()
        {
            _fixture = new CatalogService(new CatalogWebApplicationFactory<Catalog.API.Startup>());
        }
        [Fact]
        public async void Test01()//Product_UploadCSV_Valid
        {
            // Arrange
            string url = "/api/v1/AdminProduct/";
            string paramString = "Category,SubCategory,ManufacturerPartNo,Manufacturer,ItemName,Description,Stock,Price,Packaging,Series,DatasheetUrl,ImageUrls,AdditionalFields\r\nPassive Components,EMI / RFI Components,BLM18AG601SN1D,Murata Electronics,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0603 1LN,44,28.005,Tape & Reel (TR),\"EMIFIL®, BLM18\",https://www.murata.com/en-us/products/productdata/8796738650142/ENFA0003.pdf,https://media.digikey.com/Renders/Murata%20Renders/0603(LQG18).jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\nPassive Components,EMI / RFI Components,MMZ0603S601CT000,TDK Corporation,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0201 1LN,8,33.606,Tape & Reel (TR),MMZ,https://product.tdk.com/en/system/files?file=dam/doc/product/emc/emc/beads/catalog/beads_commercial_signal_mmz0603_en.pdf,https://media.digikey.com/Renders/TDK%20Renders/MMZ0603xxxxCT000.jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\n"; ;
            MultipartFormDataContent formdata = new MultipartFormDataContent();
            formdata.Add(new StringContent(paramString), "file", "file.csv");
            // Act
            var response1 = await _fixture.client.PostAsync(url, formdata);
            var result1 = response1.ReadContentAs<CSVDto>();
            await Console.Out.WriteLineAsync("");
            //Assert
            response1.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result1.Result.Should().BeOfType<CSVDto>().Which.NewProducts.Count().Should().Be(2);
            result1.Result.Should().BeOfType<CSVDto>().Which.UpdateProducts.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.DuplicatePartNumbers.Count().Should().Be(0);
            result1.Result.Should().BeOfType<CSVDto>().Which.InvalidEntries.Count().Should().Be(0);
            productId = result1.Result.NewProducts.FirstOrDefault().Id;

            // Arrange
            var url2 = "/api/v1/Products";

            // Act
            var response2 = await _fixture.client.GetAsync(url2);
            var result2 = response2.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response2.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result2.Result.Should().HaveCount(6);


            // Arrange
            string url3 = $"/api/v1/AdminProduct/{productId}";

            // Act
            var response3 = await _fixture.client.DeleteAsync(url3);
            var result3 = response3.ReadContentAs<IEnumerable<CategoryWithCount>>();

            // Assert
            response3.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
            result3.Result.Should().HaveCount(4);
        }
        //[Fact]
        //public async void Test02()//Product_GetCountWitCategory_Valid
        //{
        //    // Arrange




        //}
        //[Fact]
        //public async void Product_DeleteProduct_Valid()
        //{
       
        //}
    }
}

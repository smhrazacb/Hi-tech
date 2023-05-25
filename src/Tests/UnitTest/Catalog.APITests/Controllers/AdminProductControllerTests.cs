using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Catalog.APITests.TestData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Catalog.API.Controllers.Tests
{
    public class AdminProductControllerTests
    {
        private readonly Mock<IProductRepositoryW> _IProductRepositoryW;
        private readonly Mock<IProductRepositoryR> _IProductRepositoryR;
        private readonly Mock<ICSV2Category> _ICSV2Category;
        private readonly Mock<HttpContext> _HttpContext;
        public AdminProductControllerTests()
        {
            _IProductRepositoryW = new Mock<IProductRepositoryW>();
            _IProductRepositoryR = new Mock<IProductRepositoryR>();
            _ICSV2Category = new Mock<ICSV2Category>();
            _HttpContext = new Mock<HttpContext>();
            var path = new PathString("/api/v1/products");
            _HttpContext.Setup(_ => _.Request.Path).Returns(path);
        }
        [Fact()]
        public async void UplaodCSVProducts_Valid()
        {
            // Arrange
            var newPorducts = new List<Category>(){
                ProductData.GetPreconfiguredProducts().ElementAt(2),
                ProductData.GetPreconfiguredProducts().ElementAt(3) };

            var invalidProducts = new List<string>()
            {
                JsonConvert.SerializeObject(ProductData.GetPreconfiguredProducts().ElementAt(4)),
            };

            var duplicatePartnumber = new List<string>()
            {
                ProductData.GetPreconfiguredProducts().First().SubCategory.Product.ManufacturerPartNo
            };

            //Setup mock file using a memory stream
            var content =
                "Category,SubCategory,ManufacturerPartNo,Manufacturer,ItemName,Description,Stock,Price,Packaging,Series,DatasheetUrl,ImageUrls,AdditionalFields\r\nPassive Components,EMI / RFI Components,BLM18AG601SN1D,Murata Electronics,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0603 1LN,44,28.005,Tape & Reel (TR),\"EMIFIL®, BLM18\",https://www.murata.com/en-us/products/productdata/8796738650142/ENFA0003.pdf,https://media.digikey.com/Renders/Murata%20Renders/0603(LQG18).jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\nPassive Components,EMI / RFI Components,MMZ0603S601CT000,TDK Corporation,Ferrite Beads and Chips,FERRITE BEAD 600 OHM 0201 1LN,8,33.606,Tape & Reel (TR),MMZ,https://product.tdk.com/en/system/files?file=dam/doc/product/emc/emc/beads/catalog/beads_commercial_signal_mmz0603_en.pdf,https://media.digikey.com/Renders/TDK%20Renders/MMZ0603xxxxCT000.jpg,\"[{MountingType,Surface Mount},{Package/Case,IND 0603 }]\"\r\n";
            var fileName = "test.csv";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            CSVDto cSVDto = new CSVDto()
            {
                DuplicatePartNumbers = duplicatePartnumber,
                InvalidEntries = invalidProducts,
                NewProducts = newPorducts,
            };

            _ICSV2Category.Setup(x => x.Read(It.IsAny<string>())).
                Returns(cSVDto);

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            _IProductRepositoryR.SetupSequence(x => x.GetProductsByMFP(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts()
                .Where(x => x.SubCategory.Product.ManufacturerPartNo == newPorducts[0].SubCategory.Product.Manufacturer))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts()
                .Where(x => x.SubCategory.Product.ManufacturerPartNo == newPorducts[1].SubCategory.Product.Manufacturer));

            _IProductRepositoryW.Setup(x => x.UploadProducts(It.IsAny<IEnumerable<Category>>()));

            // Act
            AdminProductController apc = new AdminProductController(
                _IProductRepositoryR.Object, _IProductRepositoryW.Object, _ICSV2Category.Object);
            var result = await apc.UplaodCSVProducts(file);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<CSVDto>().Which.NewProducts.Count().Should().Be(2);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<CSVDto>().Which.UpdateProducts.Count().Should().Be(0);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<CSVDto>().Which.DuplicatePartNumbers.Count().Should().Be(1);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<CSVDto>().Which.InvalidEntries.Count().Should().Be(1);
        }

        [Fact()]
        public void UpdateProductTest()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();

            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).ReturnsAsync(product);
            _IProductRepositoryW.Setup(x => x.UpdateProduct(It.IsAny<Category>())).ReturnsAsync(true);
            // Act
            AdminProductController apc = new AdminProductController(
                _IProductRepositoryR.Object, _IProductRepositoryW.Object, _ICSV2Category.Object);
            var result = apc.UpdateProduct(product);
            // Assert

            result.Result.Should().BeOfType<NoContentResult>().Which.StatusCode.Should().Be(204);
        }
    }
}
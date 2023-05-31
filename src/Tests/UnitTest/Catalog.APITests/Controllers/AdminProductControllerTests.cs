using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using TestData;
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



            CSVDto cSVDto = new CSVDto()
            {
                DuplicatePartNumbers = duplicatePartnumber,
                InvalidEntries = invalidProducts,
                NewProducts = newPorducts,
            };

            _ICSV2Category.Setup(x => x.Read(It.IsAny<string>())).
                Returns(cSVDto);

            _IProductRepositoryR.SetupSequence(x => x.GetProductsByMFP(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts()
                .Where(x => x.SubCategory.Product.ManufacturerPartNo == newPorducts[0].SubCategory.Product.Manufacturer))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts()
                .Where(x => x.SubCategory.Product.ManufacturerPartNo == newPorducts[1].SubCategory.Product.Manufacturer));

            _IProductRepositoryW.Setup(x => x.UploadProducts(It.IsAny<IEnumerable<Category>>()));

            // Act
            AdminProductController apc = new AdminProductController(
                _IProductRepositoryR.Object, _IProductRepositoryW.Object, _ICSV2Category.Object);
            var result = await apc.UplaodCSVProducts(ProductData.GetFile());

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

        [Fact()]
        public void UpdateProductTest_ReturnNotFound()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();

            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).Returns(Task.FromResult((Category)null));
            _IProductRepositoryW.Setup(x => x.UpdateProduct(It.IsAny<Category>())).ReturnsAsync(true);
            // Act
            AdminProductController apc = new AdminProductController(
                _IProductRepositoryR.Object, _IProductRepositoryW.Object, _ICSV2Category.Object);
            var result = apc.UpdateProduct(product);
            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact()]
        public void UpdateProductTest_ReturnBadRequest()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();
            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).ReturnsAsync(product);
            _IProductRepositoryW.Setup(x => x.UpdateProduct(It.IsAny<Category>())).ReturnsAsync(false);
            // Act
            AdminProductController apc = new AdminProductController(
                _IProductRepositoryR.Object, _IProductRepositoryW.Object, _ICSV2Category.Object);
            var result = apc.UpdateProduct(product);
            // Assert

            result.Result.Should().BeOfType<BadRequestResult>();

        }

        [Fact()]
        public void DeleteProductByIdTest_ReturnOkResult()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();

            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).ReturnsAsync(product);
            _IProductRepositoryW.Setup(x => x.DeleteProduct(product.Id)).ReturnsAsync(true);
            // Act
            AdminProductController apc = new AdminProductController(_IProductRepositoryR.Object,
                _IProductRepositoryW.Object, _ICSV2Category.Object);
            //var result = apc.UpdateProduct(product);
            var result = apc.DeleteProductById(product.Id);
            // Assert

            result.Result.Should().BeOfType<OkResult>().Which.StatusCode.Should().Be(200);
        }

        [Fact()]
        public void DeleteProductByIdTest_ReturnNotFound()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();

            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).Returns(Task.FromResult((Category)null));
            _IProductRepositoryW.Setup(x => x.DeleteProduct(product.Id)).ReturnsAsync(true);
            // Act
            AdminProductController apc = new AdminProductController(_IProductRepositoryR.Object,
                _IProductRepositoryW.Object, _ICSV2Category.Object);
            //var result = apc.UpdateProduct(product);
            var result = apc.DeleteProductById(product.Id);
            // Assert

            result.Result.Should().BeOfType<NotFoundResult>();

        }

        [Fact()]
        public void DeleteProductByIdTest_ReturnBadRequest()
        {
            // Arrange
            var product = ProductData.GetPreconfiguredProducts().First();

            _IProductRepositoryR.Setup(x => x.GetProductById(It.IsAny<string>())).ReturnsAsync(product);
            _IProductRepositoryW.Setup(x => x.DeleteProduct(product.Id)).ReturnsAsync(false);
            // Act
            AdminProductController apc = new AdminProductController(_IProductRepositoryR.Object,
                _IProductRepositoryW.Object, _ICSV2Category.Object);
            //var result = apc.UpdateProduct(product);
            var result = apc.DeleteProductById(product.Id);
            // Assert

            result.Result.Should().BeOfType<BadRequestResult>();


        }

    }
}


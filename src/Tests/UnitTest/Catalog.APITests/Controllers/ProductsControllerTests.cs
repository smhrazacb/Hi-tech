using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Catalog.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Catalog.API.Data;
using Catalog.API.Entities;
using EventBus.Messages.Common;
using System.Net;
using Catalog.APITests.TestData;
using Catalog.API.Filter;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Catalog.API.Responses;

namespace Catalog.API.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepositoryR> _IProductRepositoryR;
        private readonly Mock<IUriService> _IUriService;
        private readonly Mock<HttpContext> _HttpContext;

        public ProductsControllerTests()
        {
            _IProductRepositoryR = new Mock<IProductRepositoryR>();
            _IUriService = new Mock<IUriService>();
            _HttpContext = new Mock<HttpContext>();
            var path = new PathString("/api/v1/products");
            _HttpContext.Setup(_ => _.Request.Path).Returns(path);

        }

        [Fact]
        public async void Product_GetCountWitCategory_Valid()
        {
            _IProductRepositoryR.Setup(repo => repo.GetProducts()).ReturnsAsync(ProductData.GetTestProductsCount());

            ProductsController pc = new ProductsController(_IProductRepositoryR.Object, Mock.Of<IUriService>());
            var result = await pc.Products();

            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<List<CategoryWithCount>>().Which.Should().HaveCount(4);
        }

        [Fact()]
        public async void Product_GetById_Valid()
        {
            //Arrange
            string id = "1";
            _IProductRepositoryR.Setup(repo => repo.GetProductById(It.IsAny<string>()))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts().Where(x => x.Id == id).FirstOrDefault());

            //Act
            ProductsController pc = new ProductsController(_IProductRepositoryR.Object, Mock.Of<IUriService>());
            var result = await pc.Product(id);

            //Assert        
            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should()
                .BeOfType<ResponseMessage<Category>>().Which.Data.Id.Should().Be(id);
        }

        [Fact()]
        public async void Product_GetById_NotFound()
        {
            //Arrange
            string id = "6";
            _IProductRepositoryR.Setup(repo => repo.GetProductById(It.IsAny<string>()))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts().Where(x => x.Id == id).FirstOrDefault());

            //Act
            ProductsController pc = new ProductsController(_IProductRepositoryR.Object, Mock.Of<IUriService>());
            var result = await pc.Product(id);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should()
                .BeOfType<ResponseMessage<Category>>().Which.Message.Should().Be("NotFound");

        }
        // unit test of GetFileteredProducts
        [Fact()]
        public async void GetFileteredProducts()
        {
            // Arrange
            var pagefilter = new PaginationFilter() { PageNumber = 1, PageSize = 6 };
            var myfilter = new FilterSortDto()
            {
                Filters = new List<FilterDto>()
                    {
                    new FilterDto(){ Filterby = CEnums.PorductAttrib.CategoryName, FilterValue= "Mechanical"},
                    new FilterDto(){ Filterby = CEnums.PorductAttrib.SubCategoryName, FilterValue= "Tools"},
                    },
                Sortdto = new SortDto() { Orderby = CEnums.PorductAttrib.Stock, IsAccending = false }
            };

            var data = new FilterResult()
            {
                TotalRecords = ProductData.GetPreconfiguredProducts().Count(),
                Items = ProductData.GetPreconfiguredProducts().Where(x => x.CategoryName == "Mechanical" && x.SubCategory.SubCategoryName == "Tools")
            };

            _IProductRepositoryR.Setup(repo =>
            repo.GetFilteredProducts(It.IsAny<PaginationFilter>(), It.IsAny<FilterSortDto>()))
                .Returns(Task.FromResult(data));

            _IUriService.Setup(uri => uri.GetPageUri(pagefilter, ""));

            // Act
            ProductsController pc = new ProductsController(_IProductRepositoryR.Object, Mock.Of<IUriService>());
            pc.ControllerContext.HttpContext = _HttpContext.Object;
            var result = await pc.GetFileteredProducts(new GetbyItemDto() { Paginationfilter = pagefilter, FilterSortdto = myfilter });

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<PagedResponse<IEnumerable<Category>>>().Which.Succeeded.Should().Be(true);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<PagedResponse<IEnumerable<Category>>>().Which.Data.FirstOrDefault().Id.Should().Be("1");
        }
    }
}
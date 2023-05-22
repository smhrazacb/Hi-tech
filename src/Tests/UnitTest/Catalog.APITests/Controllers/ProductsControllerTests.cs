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

namespace Catalog.API.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        [Fact]
        public async void Product_GetCountWitCategory_Valid()
        {
            Mock<IProductRepositoryR> mockRepor = new Mock<IProductRepositoryR>();
            mockRepor.Setup(repo => repo.GetProducts()).ReturnsAsync(ProductData.GetTestProductsCount());

            ProductsController pc = new ProductsController(mockRepor.Object, Mock.Of<IUriService>());
            var result = await pc.Products();

            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<List<CategoryWithCount>>().Which.Should().HaveCount(4);
        }

        [Fact()]
        public async void Product_GetById_Valid()
        {
            string id = "1";
            Mock<IProductRepositoryR> mockRepor = new Mock<IProductRepositoryR>();
            mockRepor.Setup(repo => repo.GetProductById(id))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts().Where(x => x.Id == id).FirstOrDefault());

            ProductsController pc = new ProductsController(mockRepor.Object, Mock.Of<IUriService>());
            var result = await pc.Product(id);

            // generate unit test         
            result.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should()
                .BeOfType<ResponseMessage<Category>>().Which.Data.Id.Should().Be(id);
        }

        [Fact()]
        public async void Product_GetById_NotFound()
        {
            string id = "5";
            Mock<IProductRepositoryR> mockRepor = new Mock<IProductRepositoryR>();
            mockRepor.Setup(repo => repo.GetProductById(id))
                .ReturnsAsync(ProductData.GetPreconfiguredProducts().Where(x => x.Id == id).FirstOrDefault());

            ProductsController pc = new ProductsController(mockRepor.Object, Mock.Of<IUriService>());
            var result = await pc.Product(id);

            // generate unit test                 
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

            var (a, b) = ((long)ProductData.GetPreconfiguredProducts().Count(),
                ProductData.GetPreconfiguredProducts().Where(x => x.CategoryName == "Mechanical" && x.SubCategory.SubCategoryName == "Tools"));

            Mock<IProductRepositoryR> mockRepor = new Mock<IProductRepositoryR>();
            mockRepor.Setup(repo => repo.GetFilteredProducts(pagefilter, myfilter)).
                ReturnsAsync(new FilterResult() {TotalRecords=5, Items= ProductData.GetPreconfiguredProducts()});


            var mockUriService = new Mock<IUriService>();
            mockUriService.Setup(uri => uri.GetPageUri(pagefilter, ""));
            ProductsController pc = new ProductsController(mockRepor.Object, Mock.Of<IUriService>());

            // Act

            var result = await pc.GetFileteredProducts(new GetbyItemDto() { Paginationfilter = pagefilter, FilterSortdto = myfilter });

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<List<CategoryWithCount>>().Which.Should().HaveCount(2);
        }


    }
}
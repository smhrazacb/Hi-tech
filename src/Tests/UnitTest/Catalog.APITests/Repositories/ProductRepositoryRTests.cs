using Xunit;
using Catalog.APITests.Fixtures;
using FluentAssertions;

namespace Catalog.API.Repositories.Tests
{
    public class ProductRepositoryRTests : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;

        public ProductRepositoryRTests(DbFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact()]
        public async void GetProductsTest()
        {	
            //arrange
            var sut = new ProductRepositoryR(_fixture.DbContext);

            //act
            var products = await sut.GetProducts();

            //assert
            products.Should().HaveCount(4);
        }

        //[Fact()]
        //public void GetProductByIdTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}

        //[Fact()]
        //public void GetFilteredProductsTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}

        //[Fact()]
        //public void GetProductsByMFPTest()
        //{
        //    Assert.True(false, "This test needs an implementation");
        //}
    }
}
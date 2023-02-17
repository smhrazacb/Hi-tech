using Microsoft.AspNetCore.Mvc;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryWithCount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetProducts()
        {
            var products = await repository.GetCategoryList();
            return Ok(products);
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    repository.GetCategoryList();

        //    var products = await repository.GetProducts();
        //    return Ok(products);
        //}


        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsById(string id)
        {
            var products = await repository.GetProductsById(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Route("[action]/{category}", Name = "GetProductsByCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsByCategory(string catogory)
        {
            var products = await repository.GetProductsByCategory(catogory);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Route("[action]/{subcategory}", Name = "GetProductsBySubCategory")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsBySubCategory(string subCatogory)
        {
            var products = await repository.GetProductsBySubCategory(subCatogory);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [Route("[action]/{name}", Name = "GetProductsByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsByName(string name)
        {
            var products = await repository.GetProductsByName(name);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Returns All Category, Subcategory and SucCategory Counts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryWithCount>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CategoryWithCount>>> Products()
        {
            var products = await repository.GetProducts();
            return Ok(products);
        }
        /// <summary>
        /// Return a Product if Id matched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name= "Product")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> Product(string id)
        {
            var products = await repository.GetProductsById(id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        /// <summary>
        /// Returns list of Product detail if Category matched
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> Category(string category)
        {
            var products = await repository.GetProductsByCategory(category);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        /// <summary>
        /// Returns list of Product detail if SubCategory matched
        /// </summary>
        /// <param name="subCategory"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> SubCategory(string subCategory)
        {
            var products = await repository.GetProductsBySubCategory(subCategory);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        /// <summary>
        /// Returns list of Product detail if Name matched
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> Name(string name)
        {
            var products = await repository.GetProductsByName(name);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        /// <summary>
        /// Register a new Product 
        /// </summary>
        /// <param name="productdto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Category>> Product([FromBody] CategoryDto productdto)
        {
            var product = _mapper.Map<Category>(productdto);

            await repository.CreateProduct(product);

            return CreatedAtRoute("Product", new { id = product.Id }, product);
        }
        /// <summary>
        /// Update a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Category product)
        {
            return Ok(await repository.UpdateProduct(product));
        }
        /// <summary>
        /// Delete a Product if Id matched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await repository.DeleteProduct(id));
        }
    }
}

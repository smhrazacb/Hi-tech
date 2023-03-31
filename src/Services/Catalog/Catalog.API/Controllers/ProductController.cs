using Microsoft.AspNetCore.Mvc;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;
using System;
using Catalog.API.Utilities;
using Catalog.API.Services;
using Catalog.API.Filter;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepositoryR repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepositoryR repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
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
            var products = await repository.GetProductById(id);
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
        public async Task<ActionResult<IEnumerable<Category>>> Category([FromQuery] PaginationFilter filter, string category)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var (totalPages, products)= await repository.GetProductsByCategory(validFilter, category);
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

       
    }
}

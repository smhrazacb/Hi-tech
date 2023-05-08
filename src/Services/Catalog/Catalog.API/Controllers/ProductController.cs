using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Filter;
using Catalog.API.Helpers;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Responses;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepositoryR repository;
        private readonly IUriService uriService;

        public ProductsController(IProductRepositoryR repository, IUriService uriService)
        {
            this.repository = repository;
            this.uriService = uriService;
        }
        /// <summary>
        /// Returns All Category, Subcategory and SucCategory Counts
        /// </summary>
        /// <param name="orderby"></param>
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
        [HttpGet("{id:length(24)}", Name = "Product")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Category>>> Product(string id)
        {
            var products = await repository.GetProductById(id);

            if (products == null)
                return NotFound();

            return Ok(products);
        }
        /// <summary>
        /// Returns list of Product detail if Category matched Maxpage size is 50
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<Category>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<IEnumerable<Category>>>> GetFileteredProducts([FromBody] GetbyItemDto requestDto)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(requestDto.Paginationfilter.PageNumber, requestDto.Paginationfilter.PageSize);
            var (totalRecords, products) = await repository.GetFilteredProducts(validFilter, requestDto.FilterSortdto);

            if (products == null)
                return NotFound();

            var pagedReponse = PaginationHelper.CreatePagedReponse(products, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
    }
}

using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Filter;
using Catalog.API.Helpers;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Responses;
using Catalog.API.Services;
using EventBus.Messages.Common;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepositoryR _repository;
        private readonly IUriService _uriService;

        public ProductsController(IProductRepositoryR repository, IUriService uriService)
        {
            this._repository = repository;
            this._uriService = uriService;
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
            var products = await _repository.GetProducts();

            return Ok(products);
        }
        /// <summary>
        /// Return a Product if Id matched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "Product")]
        [ProducesResponseType(typeof(ResponseMessage<Category>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ResponseMessage<Category>>> Product(string id)
        {
            var products = await _repository.GetProductById(id);

            if (products == null)
                return Ok(new ResponseMessage<Category>(HttpStatusCode.NotFound.ToString()));

            return Ok(new ResponseMessage<Category>(products));
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
            var validFilter = new PaginationFilter(requestDto.Paginationfilter.PageNumber, requestDto.Paginationfilter.PageSize);
            var filterResult = await _repository.GetFilteredProducts(validFilter, requestDto.FilterSortdto);
            var route = Request.Path.Value;
            var pagedReponse = PaginationHelper.CreatePagedReponse(filterResult.Items, validFilter, filterResult.TotalRecords, _uriService, route);
            
            if (filterResult.TotalRecords == 0)
                return NotFound(pagedReponse);

            return Ok(pagedReponse);
        }
    }
}

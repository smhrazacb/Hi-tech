﻿using Microsoft.AspNetCore.Mvc;
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
using Catalog.API.Helpers;
using Microsoft.AspNetCore.Routing;
using Catalog.API.Responses;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepositoryR repository;
        private readonly IMapper _mapper;
        private readonly IUriService uriService;

        public ProductsController(IProductRepositoryR repository, IMapper mapper, IUriService uriService)
        {
            this.repository = repository;
            _mapper = mapper;
            this.uriService = uriService;
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
        /// Returns list of Product detail if Category matched Maxpage size is 50
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<Category>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<IEnumerable<Category>>>> Category([FromQuery] PaginationFilter filter, string category)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var (totalRecords, products)= await repository.GetProductsByCategory(validFilter, category);
            if (products == null)
            {
                return NotFound();
            }
            var pagedReponse = PaginationHelper.CreatePagedReponse(products, validFilter, totalRecords, uriService, route);

            return Ok(pagedReponse);
        }
        /// <summary>
        /// Returns list of Product detail if SubCategory matched Maxpage size is 50
        /// </summary>
        /// <param name="subCategory"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<Category>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<IEnumerable<Category>>>> SubCategory([FromQuery] PaginationFilter filter, string subCategory)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var (totalRecords, products) = await repository.GetProductsBySubCategory(validFilter, subCategory);
            if (products == null)
            {
                return NotFound();
            }
            var pagedReponse = PaginationHelper.CreatePagedReponse(products, validFilter, totalRecords, uriService, route);

            return Ok(pagedReponse);
        }
        /// <summary>
        /// Returns list of Product detail if Name matched Maxpage size is 50
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("[action]/")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<Category>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedResponse<IEnumerable<Category>>>> Name([FromQuery] PaginationFilter filter, string name)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var (totalRecords, products) = await repository.GetProductsByName(validFilter, name);
            if (products == null)
            {
                return NotFound();
            }
            var pagedReponse = PaginationHelper.CreatePagedReponse(products, validFilter, totalRecords, uriService, route);

            return Ok(pagedReponse);
        }

       
    }
}

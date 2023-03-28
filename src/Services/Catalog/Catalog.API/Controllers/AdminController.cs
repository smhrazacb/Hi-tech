using AutoMapper;
using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper _mapper;
        public AdminController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Uplaod or update products via CSV file
        /// </summary>
        /// <param name="csvfilepath"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CSVDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CSVDto>> UplaodUpdateProducts([FromBody] string csvfilepath)
        {
            var Fname = @"C:\Users\SmhrazaEspark\Downloads\StockData Edited.csv";
            var cr = new CSV2Category();
            var result = cr.Read(Fname);
            // Check already existed products 
            foreach (var item in result.NewCategories)
            {
                var res = repository
                    .GetProductsByMFP(item.SubCategory.Product.ManufacturerPartNo,
                    item.SubCategory.Product.Manufacturer);
                if (res.Result.Count() != 0)
                {
                    item.Id = res.Result.FirstOrDefault().Id;
                    result.UpdateCategories.Add(item);
                }
            }
            // Update already existed products 
            if (result.UpdateCategories.Count() != 0)
                await repository.UpdateProducts(result.UpdateCategories);

            foreach (var item in result.UpdateCategories)
            {
                result.NewCategories.Remove(item);
            }
            // Upload New Products
            if (result.NewCategories.Count() != 0)
                await repository.UploadProducts(result.NewCategories);

            return Ok(result);
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

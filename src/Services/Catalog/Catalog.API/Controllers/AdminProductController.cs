using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OpenIddict.Validation.AspNetCore;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class AdminProductController : ControllerBase
    {
        private readonly IProductRepositoryR repositoryR;
        private readonly IProductRepositoryW repositoryW;
        private readonly ICSV2Category _csv2category;
        private readonly ILogger<AdminProductController> _logger;

        public AdminProductController(IProductRepositoryR repositoryR, IProductRepositoryW repositoryW, ICSV2Category csv2category)
        {
            this.repositoryR = repositoryR;
            this.repositoryW = repositoryW;
            _csv2category = csv2category;
        }

        /// <summary>
        /// Uplaod or update products via CSV file
        /// </summary>
        /// <param name="csvfilepath"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CSVDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<CSVDto>> UplaodCSVProducts(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("File is not selected or empty");
                // Save the file to disk
                var destPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/upload");
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);

                var filePath = Path.Combine(destPath,
                    Path.GetFileNameWithoutExtension(file.FileName)
                      + "_"
                      + DateTime.Now.ToString("yyyyddMMHHmmss")
                      + Path.GetExtension(file.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    Console.WriteLine(filePath);
                }
                var result = _csv2category.Read(filePath);

                foreach (var item in result.NewProducts)
                {
                    var res = repositoryR
                        .GetProductsByMFP(item.SubCategory.Product.ManufacturerPartNo,
                        item.SubCategory.Product.Manufacturer);
                    if (res.Result.Count() != 0)
                    {
                        item.Id = res.Result.FirstOrDefault().Id;
                        result.UpdateProducts.Add(item);
                    }
                }

                foreach (var item in result.UpdateProducts)
                {
                    result.NewProducts.Remove(item);
                }
                // Update db already existed products 
                if (result.UpdateProducts.Count() != 0)
                {
                    var bulkWriteResult = await repositoryW.UpdateProducts(result.UpdateProducts);
                }
                // Update db New Products
                if (result.NewProducts.Count() != 0)
                {
                    await repositoryW.UploadProducts(result.NewProducts);
                }
                // Populate Summary 
                result.DuplicatePartNumbersCount = result.DuplicatePartNumbers.Count();
                result.InvalidEntriesCount = result.InvalidEntries.Count();
                result.NewProductsCount = result.NewProducts.Count();
                result.UpdateProductsCount = result.UpdateProducts.Count();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Update a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProduct([FromBody] Category product)
        {
            if (await repositoryR.GetProductById(product.Id) == null)
                return NotFound();
            if (await repositoryW.UpdateProduct(product))
            {
                return NoContent();
            }
            return BadRequest();
        }
        /// <summary>
        /// Delete a Product if Id matched
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            if (await repositoryR.GetProductById(id) == null)
                return NotFound();
            if (await repositoryW.DeleteProduct(id))
                return Ok();
            return BadRequest();
        }
    }
}

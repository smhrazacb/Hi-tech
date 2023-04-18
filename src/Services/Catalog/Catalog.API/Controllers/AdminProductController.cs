﻿using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class AdminProductController : ControllerBase
    {
        private readonly IProductRepositoryR repositoryR;
        private readonly IProductRepositoryW repositoryW;
        private readonly ICSV2Category _csv2category;

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
        public async Task<ActionResult<CSVDto>> UplaodCSVProducts(IFormFile file)
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
                var res = await repositoryW.UpdateProducts(result.UpdateProducts);
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
        /// <summary>
        /// Update a Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Category), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Category product)
        {
            return Ok(await repositoryW.UpdateProduct(product));
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
            return Ok(await repositoryW.DeleteProduct(id));
        }
    }
}

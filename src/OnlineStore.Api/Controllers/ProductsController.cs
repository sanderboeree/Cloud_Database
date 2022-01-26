using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using OnlineStore.Api.Infrastructure.ExceptionHandlers;
using System.Collections.Generic;
using OnlineStore.Api.Application.Orders;
using OnlineStore.Api.Domain.Orders;
using OnlineStore.Api.Infrastructure.Crud.Interfaces;
using OnlineStore.Api.Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using OnlineStore.Api.Application.Common;
using OnlineStore.Api.Application.Orders.Interfaces;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using OnlineStore.Api.Infrastructure.EntityFramework.Data;

namespace OnlineStore.Api.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ICrudService<Product, ProductDto> _productsCrudService;
        private readonly ICrudService<Review, ReviewDto> _reviewsCrudService;

        private readonly IProductImageFileService _productImageFileService;

        public ProductsController(ICrudService<Product, ProductDto> productsCrudService,
            ICrudService<Review, ReviewDto> reviewsCrudService,
            IProductImageFileService productsImageFileService
            )
        {
            _productsCrudService = productsCrudService;
            _reviewsCrudService = reviewsCrudService;

            _productImageFileService = productsImageFileService;
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                return BadRequest(new ApiError { ErrorCode = ErrorCode.HttpStatus400.RequiredValue, ErrorMessage = ErrorCode.HttpStatus400.RequiredValueMessage });
            }

            var product = await _productsCrudService.CreateAsync(dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpGet()]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await _productsCrudService.GetAsync(new AllNotDeleted<Product>(), cancellationToken));
        }

        /// <summary>
        /// Creates image for product
        /// </summary>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [Authorize(Policy = RoleData.Admin)]
        [HttpPost("id/images")]
        public async Task<IActionResult> CreateFileAsync(Guid id, [FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null)
            {
                return BadRequest(new ApiError { ErrorCode = ErrorCode.HttpStatus400.RequiredValue, ErrorMessage = ErrorCode.HttpStatus400.RequiredValueMessage });
            }

            await _productImageFileService.SaveAsync(id, new FileData { Filename = file.FileName, FileStream = file.OpenReadStream() }, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Update a image of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [ProducesResponseType(typeof(void), 204)]
        [Authorize(Policy = RoleData.Admin)]
        [HttpPut("{id}/images")]
        public async Task<IActionResult> UpdateFileAsync(Guid id, [FromForm] IFormFile file, CancellationToken cancellationToken)
        {
            await _productImageFileService.DeleteAsync(id, cancellationToken);
            await _productImageFileService.SaveAsync(id, new FileData { Filename = file.FileName, FileStream = file.OpenReadStream() }, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Delete a image of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [ProducesResponseType(typeof(void), 204)]
        [Authorize(Policy = RoleData.Admin)]
        [HttpDelete("{id}/images")]
        public async Task<IActionResult> DeleteFileAsync(Guid id, CancellationToken cancellationToken)
        {
            await _productImageFileService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets a single file of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [AllowAnonymous]
        [ProducesResponseType(typeof(Error), 404)]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [HttpGet("{id}/images")]
        public async Task<IActionResult> GetFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var fileData = await _productImageFileService.GetAsync(id, cancellationToken);
            return fileData.FileStream == null ? NoContent() : File(fileData.FileStream, MediaTypeNames.Application.Octet, fileData.Filename);
        }


        /// <summary>
        /// Creates image for product
        /// </summary>
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(Error), 400)]
        [ProducesResponseType(typeof(Error), 409)]
        [HttpPost("reviews")]
        public async Task<IActionResult> CreateReviewAsync([FromBody] ReviewDto dto, CancellationToken cancellationToken)
        {
            if (dto == null)
            {
                return BadRequest(new ApiError { ErrorCode = ErrorCode.HttpStatus400.RequiredValue, ErrorMessage = ErrorCode.HttpStatus400.RequiredValueMessage });
            }

            await _reviewsCrudService.CreateAsync(dto, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Update a image of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [ProducesResponseType(typeof(ReviewDto), 204)]
        [HttpPut("{id}/reviews")]
        public async Task<IActionResult> UpdateReviewAsync(Guid id, [FromBody] ReviewDto dto, CancellationToken cancellationToken)
        {
            return Ok(await _reviewsCrudService.UpdateAsync(id, dto, cancellationToken));
        }

        /// <summary>
        /// Delete a image of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [ProducesResponseType(typeof(void), 204)]
        [Authorize(Policy = RoleData.Admin)]
        [HttpDelete("{id}/reviews")]
        public async Task<IActionResult> DeleteReviewAsync(Guid id, CancellationToken cancellationToken)
        {
            await _reviewsCrudService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Gets a single file of a product
        /// </summary>  
        /// <param name="id">The id of the product</param>
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        [ProducesResponseType(typeof(Error), 404)]
        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetReviewsAsync(Guid id, CancellationToken cancellationToken)
        {
            var reviews = await _reviewsCrudService.GetAsync(new ReviewsForProduct(id), cancellationToken);
            return Ok(reviews);
        }

    }
}

using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingImages;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.DTOs;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller de Listing Images
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorization]
    public class ListingImagesController : ControllerBase
    {
        private readonly IListingImagesServices _listingImagesServices;
        private readonly ILogger<ListingImagesController> _logger;

        public ListingImagesController(
            IListingImagesServices listingImagesServices,
            ILogger<ListingImagesController> logger)
        {
            _listingImagesServices = listingImagesServices;
            _logger = logger;
        }

        /// <summary>
        /// Get all images
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListingImagesDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var images = await _listingImagesServices.GetAllAsync();
                return Ok(ApiResponse<List<ListingImagesDto>>.SuccessResponse(
                    images,
                    $"{images.Count} image(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing images");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving listing images"));
            }
        }

        /// <summary>
        /// Get image by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingImagesDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var image = await _listingImagesServices.GetByIdAsync(id);
                return Ok(ApiResponse<ListingImagesDto>.SuccessResponse(image));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing image {ImageId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving listing image"));
            }
        }

        /// <summary>
        /// Create image
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingImagesDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddListingImagesDTO dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var image = await _listingImagesServices.CreateAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = image.Id },
                    ApiResponse<ListingImagesDto>.SuccessResponse(image, "Image created successfully")
                );
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing image");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error creating listing image"));
            }
        }

        /// <summary>
        /// Update image
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingImagesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateListingImagesDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch"));

                var userId = HttpContext.GetUserId();
                var image = await _listingImagesServices.UpdateAsync(userId, dto);

                return Ok(ApiResponse<ListingImagesDto>.SuccessResponse(image, "Image updated successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                return StatusCode(403, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating listing image {ImageId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error updating listing image"));
            }
        }
    }
}

using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingHours;
using Api.Shared.DTOs;
using Api.Shared.DTOs.ListingHours;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorization]
    public class ListingHoursController : ControllerBase
    {
        private readonly IListingHoursServices _listingHoursServices;
        private readonly ILogger<ListingHoursController> _logger;

        public ListingHoursController(
            IListingHoursServices listingHoursServices,
            ILogger<ListingHoursController> logger)
        {
            _listingHoursServices = listingHoursServices;
            _logger = logger;
        }

        /// <summary>
        /// Get all listing hours
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListingHoursDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var hours = await _listingHoursServices.GetAllAsync();
                return Ok(ApiResponse<List<ListingHoursDto>>.SuccessResponse(
                    hours,
                    $"{hours.Count} record(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing hours");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving listing hours"
                ));
            }
        }

        /// <summary>
        /// Get listing hours by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingHoursDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var hour = await _listingHoursServices.GetByIdAsync(id);
                return Ok(ApiResponse<ListingHoursDto>.SuccessResponse(hour));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "ListingHour not found: {Id}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing hour {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving the listing hour"
                ));
            }
        }

        /// <summary>
        /// Create listing hours
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingHoursDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddListingHoursDTO dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var created = await _listingHoursServices.CreateAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    ApiResponse<ListingHoursDto>.SuccessResponse(created, "Listing hours created successfully")
                );
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while creating listing hours");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing hours");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating listing hours"
                ));
            }
        }

        /// <summary>
        /// Update listing hours
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingHoursDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateListingHoursDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "ID in URL doesn't match ID in body"
                    ));

                var userId = HttpContext.GetUserId();
                var updated = await _listingHoursServices.UpdateAsync(userId, dto);

                return Ok(ApiResponse<ListingHoursDto>.SuccessResponse(
                    updated,
                    "Listing hours updated successfully"
                ));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "ListingHour not found: {Id}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning(ex, "Unauthorized update attempt: {Id}", id);
                return StatusCode(403, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while updating listing hours: {Id}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating listing hours {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating listing hours"
                ));
            }
        }
    }
}

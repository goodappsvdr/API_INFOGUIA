using Api.Shared.DTOs;
using Api.Shared.DTOs.ListingServices;
using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingServices;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador de Servicios asociados a Listings
    /// </summary>
    [Route("api/listings/services")]
    [ApiController]
    [JwtAuthorization]
    public class ListingServicesController : ControllerBase
    {
        private readonly IListingServicesService _service;
        private readonly ILogger<ListingServicesController> _logger;

        public ListingServicesController(
            IListingServicesService service,
            ILogger<ListingServicesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ===================== CREATE =====================

        /// <summary>
        /// Asocia un servicio a un listing
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingServicesDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] AddListingServicesDTO dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var created = await _service.CreateAsync(userId, dto);

                return Created(
                    string.Empty,
                    ApiResponse<ListingServicesDto>.SuccessResponse(
                        created,
                        "Service added to listing successfully"
                    )
                );
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while adding service to listing");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding service to listing");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error adding service to listing"));
            }
        }

        // ===================== GET BY LISTING =====================

        /// <summary>
        /// Obtiene los servicios asociados a un listing
        /// </summary>
        [HttpGet("listing/{listingId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ListingServicesDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByListing(int listingId)
        {
            try
            {
                var services = await _service.GetByListingIdAsync(listingId);

                return Ok(ApiResponse<List<ListingServicesDto>>.SuccessResponse(
                    services,
                    $"{services.Count} service(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving services for listing {ListingId}", listingId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving listing services"));
            }
        }

        // ===================== DELETE =====================

        /// <summary>
        /// Quita un servicio de un listing
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(
            [FromQuery] int listingId,
            [FromQuery] int serviceId)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _service.DeleteAsync(userId, listingId, serviceId);

                return Ok(ApiResponse<object>.SuccessResponse(
                    null,
                    "Service removed from listing successfully"
                ));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing service {ServiceId} from listing {ListingId}", serviceId, listingId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error removing service from listing"));
            }
        }
    }
}

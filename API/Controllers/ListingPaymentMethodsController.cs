using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingPaymentMethods;
using Api.Shared.DTOs;
using Api.Shared.DTOs.ListingPaymentMethods;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador de Elementos de Pago de Listing
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorization]
    public class ListingPaymentMethodsController : ControllerBase
    {
        private readonly IListingPaymentMethodsServices _listingPaymentMethodsServices;
        private readonly ILogger<ListingPaymentMethodsController> _logger;

        public ListingPaymentMethodsController(
            IListingPaymentMethodsServices listingPaymentMethodsServices,
            ILogger<ListingPaymentMethodsController> logger)
        {
            _listingPaymentMethodsServices = listingPaymentMethodsServices;
            _logger = logger;
        }

        /// <summary>
        /// Get all listing payment methods
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListingPaymentMethodsDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var items = await _listingPaymentMethodsServices.GetAllAsync();
                return Ok(ApiResponse<List<ListingPaymentMethodsDto>>.SuccessResponse(
                    items,
                    $"{items.Count} record(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing payment methods");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving listing payment methods"
                ));
            }
        }

        /// <summary>
        /// Get listing payment method by ListingID and PaymentMethodID
        /// </summary>
        [HttpGet("{listingId}/{paymentMethodId}")]
        [ProducesResponseType(typeof(ApiResponse<ListingPaymentMethodsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int listingId, int paymentMethodId)
        {
            try
            {
                var item = await _listingPaymentMethodsServices.GetByIdAsync(listingId, paymentMethodId);
                return Ok(ApiResponse<ListingPaymentMethodsDto>.SuccessResponse(item));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "ListingPaymentMethod not found: ListingId {ListingId}, PaymentMethodId {PaymentMethodId}",
                    listingId, paymentMethodId);

                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error retrieving listing payment method {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while retrieving listing payment method"
                ));
            }
        }

        /// <summary>
        /// Create listing payment method
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingPaymentMethodsDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] AddListingPaymentMethodsDTO dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var created = await _listingPaymentMethodsServices.CreateAsync(userId, dto);

                return Created(
                    string.Empty,
                    ApiResponse<ListingPaymentMethodsDto>.SuccessResponse(
                        created,
                        "Listing payment method created successfully"
                    )
                );
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while creating listing payment method");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing payment method");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while creating listing payment method"
                ));
            }
        }

        /// <summary>
        /// Update listing payment method
        /// </summary>
        [HttpPut("{listingId}/{paymentMethodId}")]
        [ProducesResponseType(typeof(ApiResponse<ListingPaymentMethodsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(
            int listingId,
            int paymentMethodId,
            [FromBody] UpdateListingPaymentMethodsDTO dto)
        {
            try
            {
                if (listingId != dto.ListingId || paymentMethodId != dto.Id)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "IDs in URL don't match IDs in body"
                    ));
                }

                var userId = HttpContext.GetUserId();
                var updated = await _listingPaymentMethodsServices.UpdateAsync(userId, dto);

                return Ok(ApiResponse<ListingPaymentMethodsDto>.SuccessResponse(
                    updated,
                    "Listing payment method updated successfully"
                ));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex,
                    "ListingPaymentMethod not found: {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning(ex,
                    "Unauthorized update attempt: {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                return StatusCode(403, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Bad request while updating listing payment method: {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating listing payment method {ListingId}-{PaymentMethodId}",
                    listingId, paymentMethodId);

                return StatusCode(500, ApiResponse<object>.ErrorResponse(
                    "An error occurred while updating listing payment method"
                ));
            }
        }
    }
}

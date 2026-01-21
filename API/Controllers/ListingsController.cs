using Api.Infrastructure.Services.Listings;
using Api.Shared.DTOs.Listings;
using Api.Shared.Models;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;
using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs;

namespace API.Controllers
{
    /// <summary>
    /// Controlador de Listings
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorization] // Aplica la autorización a todos los endpoints
    public class ListingsController : ControllerBase
    {
        private readonly IListingsServices _listingsServices;
        private readonly ILogger<ListingsController> _logger;

        public ListingsController(
            IListingsServices listingsServices,
            ILogger<ListingsController> logger)
        {
            _listingsServices = listingsServices;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new listing
        /// </summary>
        /// <param name="listingDto">Listing data</param>
        /// <returns>Created listing</returns>
        /// <response code="201">Listing created successfully</response>
        /// <response code="400">Invalid data</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateListing([FromBody] AddListingDTO listingDto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var createdListing = await _listingsServices.CreateListingAsync(userId, listingDto);

                return CreatedAtAction(
                    nameof(GetListingById),
                    new { id = createdListing.UserId},
                    ApiResponse<ListingDTO>.SuccessResponse(createdListing, "Listing created successfully")
                );
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while creating listing");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the listing"));
            }
        }

        /// <summary>
        /// Gets all listings
        /// </summary>
        /// <returns>List of all listings</returns>
        /// <response code="200">Returns the list of listings</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<ListingDTO>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllListings()
        {
            try
            {
                var listings = await _listingsServices.GetAllListingsAsync();
                return Ok(ApiResponse<List<ListingDTO>>.SuccessResponse(
                    listings,
                    $"{listings.Count} listing(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listings");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving listings"));
            }
        }

        /// <summary>
        /// Gets a specific listing by ID
        /// </summary>
        /// <param name="id">Listing ID</param>
        /// <returns>Listing details</returns>
        /// <response code="200">Returns the listing</response>
        /// <response code="404">Listing not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetListingById(int id)
        {
            try
            {
                var listing = await _listingsServices.GetListingByIdAsync(id);
                return Ok(ApiResponse<ListingDTO>.SuccessResponse(listing));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Listing not found: {ListingId}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving listing {ListingId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the listing"));
            }
        }

        /// <summary>
        /// Updates an existing listing
        /// </summary>
        /// <param name="id">Listing ID</param>
        /// <param name="listingDto">Updated listing data (UpdateListingDTO)</param>
        /// <returns>Updated listing</returns>
        /// <response code="200">Listing updated successfully</response>
        /// <response code="400">Invalid data</response>
        /// <response code="403">User doesn't own this listing</response>
        /// <response code="404">Listing not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        // CAMBIO: Usamos UpdateListingDTO en el Body y eliminamos el parámetro extra listingId
        public async Task<IActionResult> UpdateListing(int id, [FromBody] UpdateListingDTO listingDto)
        {
            try
            {
                // El ID viene de la URL ({id}), se lo pasamos directamente al servicio
                var userId = HttpContext.GetUserId();

                // CAMBIO: Enviamos 'id' como el identificador del listing
                var updatedListing = await _listingsServices.UpdateListingAsync(userId, listingDto, id);

                return Ok(ApiResponse<ListingDTO>.SuccessResponse(updatedListing, "Listing updated successfully"));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Listing not found: {ListingId}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning(ex, "User doesn't own listing: {ListingId}", id);
                return StatusCode(403, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while updating listing: {ListingId}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating listing {ListingId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the listing"));
            }
        }

    }
}

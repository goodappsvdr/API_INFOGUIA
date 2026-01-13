using Api.Shared.DTOs.ListingSocialLinks;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;
using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs;
using Api.Infrastructure.Services.ListingSocialLinks;

namespace API.Controllers
{
    /// <summary>
    /// Controlador de Social Links de Listings
    /// </summary>
    [Route("api/listings/social-links")]
    [ApiController]
    [JwtAuthorization]
    public class ListingSocialLinksController : ControllerBase
    {
        private readonly IListingSocialLinksServices _service;
        private readonly ILogger<ListingSocialLinksController> _logger;

        public ListingSocialLinksController(
            IListingSocialLinksServices service,
            ILogger<ListingSocialLinksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ===================== CREATE =====================

        /// <summary>
        /// Crea un nuevo social link para un listing
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ListingSocialLinksDto>), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] AddListingSocialLinksDTO dto)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var created = await _service.CreateAsync(userId, dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    ApiResponse<ListingSocialLinksDto>.SuccessResponse(created, "Social link created successfully")
                );
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request while creating social link");
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating social link");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the social link"));
            }
        }

        // ===================== GET ALL BY LISTING =====================

        /// <summary>
        /// Obtiene todos los social links de un listing
        /// </summary>
        [HttpGet("listing/{listingId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ListingSocialLinksDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByListing(int listingId)
        {
            try
            {
                var links = await _service.GetByListingIdAsync(listingId);

                return Ok(ApiResponse<List<ListingSocialLinksDto>>.SuccessResponse(
                    links,
                    $"{links.Count} social link(s) found"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving social links for listing {ListingId}", listingId);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving social links"));
            }
        }

        // ===================== GET BY ID =====================

        /// <summary>
        /// Obtiene un social link por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingSocialLinksDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var link = await _service.GetByIdAsync(id);
                return Ok(ApiResponse<ListingSocialLinksDto>.SuccessResponse(link));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Social link not found: {Id}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving social link {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving social link"));
            }
        }

        // ===================== UPDATE =====================

        /// <summary>
        /// Actualiza un social link
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<ListingSocialLinksDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateListingSocialLinksDTO dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch"));
                }

                var userId = HttpContext.GetUserId();
                var updated = await _service.UpdateAsync(userId, dto);

                return Ok(ApiResponse<ListingSocialLinksDto>.SuccessResponse(updated, "Social link updated successfully"));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Social link not found: {Id}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Bad request updating social link {Id}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating social link {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error updating social link"));
            }
        }

        // ===================== DELETE (soft) =====================

        /// <summary>
        /// Desactiva un social link
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                await _service.DeleteAsync(userId, id);

                return Ok(ApiResponse<object>.SuccessResponse(null, "Social link deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting social link {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error deleting social link"));
            }
        }
    }
}

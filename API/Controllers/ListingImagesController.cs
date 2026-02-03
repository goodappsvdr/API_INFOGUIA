using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.ListingImages;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.DTOs;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;
using Api.Shared.Data;
using Api.Shared.Models;
using System.IO;

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
        private readonly Context _context; // 👈 añadimos el DbContext

        public ListingImagesController(
            IListingImagesServices listingImagesServices,
            ILogger<ListingImagesController> logger,
             Context context)
        {
            _listingImagesServices = listingImagesServices;
            _logger = logger;
            _context = context;
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

        /// <summary>
        /// Crear Imagen de Listing
        /// </summary>
        [HttpPost("CreateListingImageUrl")]
        public async Task<ActionResult> CreateListingImageUrlAsync([FromForm] ListingImageUploadRequest request)
        {
            // 1. Validaciones de seguridad
            if (request.ImageFile == null || request.ImageFile.Length == 0)
            {
                return BadRequest("No se ha proporcionado ninguna imagen.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Formato de imagen no permitido (solo JPG, PNG, WebP).");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            string fullPath = string.Empty;

            try
            {
                // 2. Preparar el nombre y rutas
                var fileName = $"listing_{request.ListingId}_{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "Imagenes", "Listings");
                fullPath = Path.Combine(folderPath, fileName);

                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }

                // 3. Guardar archivo físico
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                // 4. Construir URL dinámica (se adapta a api.infoguia.online o localhost)
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var imageUrl = $"{baseUrl}/Imagenes/Listings/{fileName}";

                // 5. Mapear al modelo de base de datos
                var model = new ListingImage
                {
                    ListingId = request.ListingId,
                    ImageUrl = imageUrl,
                    Caption = request.Caption,
                    SortOrder = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = request.UserId.ToString()
                };

                _context.ListingImages.Add(model);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    ListingImageId = model.ListingImageId,
                    imageUrl = imageUrl
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Limpieza: si falló la DB pero el archivo se guardó, lo borramos
                if (!string.IsNullOrEmpty(fullPath) && System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                return BadRequest($"Error interno: {ex.Message}");
            }
        }
    }
}

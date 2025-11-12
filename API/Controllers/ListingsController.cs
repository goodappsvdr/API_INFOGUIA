using Microsoft.AspNetCore.Mvc;
using Api.Shared.Data;
using Api.Shared.DTOs.Listing;
using Api.Shared.Models;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OutputCaching;
using Api.Infrastructure.Services.Province;
using Api.Infrastructure.Services.Listings;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IListingsServices _listingsService;
        private readonly Context _context;

        public ListingsController(IListingsServices listingService, Context context)
        {
            _listingsService = listingService;
            _context = context;
        }

        /// <summary>
        /// Crear un nuevo Listing
        /// </summary>
        [HttpPost("CreateListing")]
        public async Task<ActionResult> CreateListingAsync([FromBody] Listing_input input)
        {
            try
            {
                if (input == null)
                    return BadRequest("El modelo de entrada es nulo.");

                var model = new Listing
                {
                    TenantId = input.TenantId,
                    CategoryId = input.CategoryId,
                    CityId = input.CityId,
                    Name = input.Name,
                    ShortDescription = input.ShortDescription,
                    LongDescription = input.LongDescription,
                    LogoUrl = input.LogoUrl,
                    Address = input.Address,
                    Latitude = input.Latitude,
                    Longitude = input.Longitude,
                    Email = input.Email,
                    WebsiteUrl = input.WebsiteUrl,
                    VideoUrl = input.VideoUrl,
                    CatalogUrl = input.CatalogUrl,
                    SortOrder = input.SortOrder,
                    IsActive = input.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = input.CreatedByUserId,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedByUserId = input.CreatedByUserId
                };

                _context.Listings.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Listing creado exitosamente.",
                    data = model
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al crear el Listing.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Modificar un Listing existente
        /// </summary>
        [HttpPut("UpdateListing/{listingId}")]
        public async Task<ActionResult> UpdateListingAsync(int listingId, [FromBody] Listing_input input)
        {
            try
            {
                if (input == null)
                    return BadRequest("El modelo de entrada es nulo.");

                var existing = await _context.Listings.FirstOrDefaultAsync(x => x.ListingId == listingId);

                if (existing == null)
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró un Listing con Id {listingId}."
                    });

                // 🔄 Mantener valores que NO se deben tocar
                var tenantId = existing.TenantId;
                var createdAt = existing.CreatedAt;
                var createdByUserId = existing.CreatedByUserId;

                // 🧩 Actualizar campos permitidos
                existing.CategoryId = input.CategoryId;
                existing.CityId = input.CityId;
                existing.Name = input.Name;
                existing.ShortDescription = input.ShortDescription;
                existing.LongDescription = input.LongDescription;
                existing.LogoUrl = input.LogoUrl;
                existing.Address = input.Address;
                existing.Latitude = input.Latitude;
                existing.Longitude = input.Longitude;
                existing.Email = input.Email;
                existing.WebsiteUrl = input.WebsiteUrl;
                existing.VideoUrl = input.VideoUrl;
                existing.CatalogUrl = input.CatalogUrl;
                existing.SortOrder = input.SortOrder;
                existing.IsActive = input.IsActive;
                existing.ModifiedAt = DateTime.UtcNow;
                existing.ModifiedByUserId = input.ModifiedByUserId;

                // 💾 Guardar cambios
                _context.Listings.Update(existing);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Listing modificado exitosamente.",
                    data = existing
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al modificado el Listing.",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Eliminar un Listing
        /// </summary>
        [HttpDelete("DeleteListing/{listingId}")]
        public async Task<ActionResult> DeleteListingAsync(int id)
        {
            try
            {
                var existing = await _context.Listings.FirstOrDefaultAsync(x => x.ListingId == id);

                if (existing == null)
                    return NotFound(new
                    {
                        success = false,
                        message = $"No se encontró un Listing con Id {id}."
                    });

                _context.Listings.Remove(existing);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Listing eliminado exitosamente."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Error al eliminar el Listing.",
                    error = ex.Message
                });
            }
        }


        /// <summary>
        /// Buscar Todos
        /// </summary>
        [AllowAnonymous]
        [HttpGet("GetAllListing")]
        [OutputCache(PolicyName = "listings")]
        public async Task<ActionResult> GetAllListingsAsync()
        {
            try
            {
                var result = await _listingsService.GetAllListingsAsync();

                if (result == null || !result.Any())
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Error al obtener las Listings.",
                    error = ex.Message
                });
            }
        }



    }
}

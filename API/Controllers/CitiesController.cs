using Api.Infrastructure.Jwt;
using Api.Infrastructure.Services.Cities;
using Api.Shared.DTOs.Cities;
using Api.Shared.Models;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesServices _citiesService;
        private readonly IOutputCacheStore _outputCacheStore;

        public CitiesController(ICitiesServices citiesService, IOutputCacheStore outputCacheStore)
        {
            _citiesService = citiesService;
            _outputCacheStore = outputCacheStore;
        }

        /// <summary>
        /// Obtiene las localidades por el id de la provincia
        /// </summary>
        /// <param name="provinceId">ID de la provincia</param>
        /// <returns>Localidades correspondientes a la provincia</returns>
        [AllowAnonymous]
        [HttpGet("{provinceId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Cities_Get>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "localidades")]
        public async Task<ActionResult<IEnumerable<Cities_Get>>> GetAsync(int provinceId)
        {
            try
            {
                var result = await _citiesService.GetAsync(provinceId);

                if (result == null || !result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al procesar la solicitud.");
            }
        }
    }
}
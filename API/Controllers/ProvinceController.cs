using Api.Infrastructure.Jwt;
using Api.Infrastructure.Services.Province;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceServices _provinceService;
        private readonly IOutputCacheStore _outputCacheStore;

        public ProvinceController(IProvinceServices provinceService, IOutputCacheStore outputCacheStore)
        {
            _provinceService = provinceService;
            _outputCacheStore = outputCacheStore;
        }

        /// <summary>
        /// Obtiene las provincias
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous, HttpGet("Get")]
        [OutputCache(PolicyName = "provincias")]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                var result = await _provinceService.GetAsync();
                result.OrderBy(x => x.Name);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}

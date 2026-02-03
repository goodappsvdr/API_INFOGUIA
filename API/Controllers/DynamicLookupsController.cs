//using Api.Infrastructure.Services.Dynamic;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace API.Controllers
//{
//    /// <summary>
//    /// Controller for lookup data and utilities
//    /// </summary>
//    [ApiController]
//    [Route("api/dynamic-lookups")]
//    [Authorize]
//    public class DynamicLookupsController : ControllerBase
//    {
//        private readonly IDynamicEntityService _entityService;
//        private readonly ILogger<DynamicLookupsController> _logger;

//        public DynamicLookupsController(
//            IDynamicEntityService entityService,
//            ILogger<DynamicLookupsController> logger)
//        {
//            _entityService = entityService;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Gets lookup data for a specific table
//        /// </summary>
//        /// <param name="tableName">Table name</param>
//        /// <param name="keyColumn">Key column</param>
//        /// <param name="displayColumn">Display column</param>
//        /// <param name="searchTerm">Search term</param>
//        /// <returns>Lookup data</returns>
//        [HttpGet]
//        public async Task<ActionResult<List<dynamic>>> GetLookupData(
//            [FromQuery] string tableName,
//            [FromQuery] string keyColumn,
//            [FromQuery] string displayColumn,
//            [FromQuery] string searchTerm = null)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(keyColumn) || string.IsNullOrEmpty(displayColumn))
//                {
//                    return BadRequest("tableName, keyColumn, and displayColumn are required");
//                }

//                var lookupData = await _entityService.GetLookupDataAsync(tableName, keyColumn, displayColumn, searchTerm);
//                return Ok(lookupData);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting lookup data for table {TableName}", tableName);
//                return StatusCode(500, "Internal server error");
//            }
//        }
//    }
//}

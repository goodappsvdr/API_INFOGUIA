//using Api.Infrastructure.Exceptions;
//using Api.Infrastructure.Services.Dynamic;
//using Api.Shared.DTOs.Dynamic;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace API.Controllers
//{
//    /// <summary>
//    /// Controller for dynamic entity CRUD operations
//    /// </summary>
//    [ApiController]
//    [Route("api/modules/{moduleId}/entities")]
//    [Authorize]
//    public class DynamicEntitiesController : ControllerBase
//    {
//        private readonly IDynamicEntityService _entityService;
//        private readonly IDynamicModuleService _moduleService;
//        private readonly ILogger<DynamicEntitiesController> _logger;

//        public DynamicEntitiesController(
//            IDynamicEntityService entityService,
//            IDynamicModuleService moduleService,
//            ILogger<DynamicEntitiesController> logger)
//        {
//            _entityService = entityService;
//            _moduleService = moduleService;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Gets entities for a module with pagination and filtering
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="pageNumber">Page number (default: 1)</param>
//        /// <param name="pageSize">Page size (default: 10)</param>
//        /// <param name="searchTerm">Search term</param>
//        /// <param name="sortField">Sort field</param>
//        /// <param name="sortAscending">Sort ascending (default: true)</param>
//        /// <returns>Paginated entity list</returns>
//        [HttpGet]
//        public async Task<ActionResult<DynamicEntityListResponseDTO>> GetEntities(
//            int moduleId,
//            [FromQuery] int pageNumber = 1,
//            [FromQuery] int pageSize = 10,
//            [FromQuery] string searchTerm = null,
//            [FromQuery] string sortField = null,
//            [FromQuery] bool sortAscending = true)
//        {
//            try
//            {
//                var searchDto = new DynamicEntitySearchDTO
//                {
//                    PageNumber = pageNumber,
//                    PageSize = Math.Min(pageSize, 100), // Limitar tamaño máximo
//                    SearchTerm = searchTerm,
//                    SortField = sortField,
//                    SortAscending = sortAscending
//                };

//                var result = await _entityService.GetEntitiesAsync(moduleId, searchDto);
//                return Ok(result);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving entities for module {ModuleId}", moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets entities with advanced filtering
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="searchDto">Search parameters</param>
//        /// <returns>Paginated entity list</returns>
//        [HttpPost("search")]
//        public async Task<ActionResult<DynamicEntityListResponseDTO>> SearchEntities(
//            int moduleId,
//            [FromBody] DynamicEntitySearchDTO searchDto)
//        {
//            try
//            {
//                // Validar tamaño de página
//                searchDto.PageSize = Math.Min(searchDto.PageSize, 100);
//                if (searchDto.PageNumber <= 0) searchDto.PageNumber = 1;

//                var result = await _entityService.GetEntitiesAsync(moduleId, searchDto);
//                return Ok(result);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error searching entities for module {ModuleId}", moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets a specific entity by ID
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityId">Entity ID</param>
//        /// <returns>Entity details</returns>
//        [HttpGet("{entityId}")]
//        public async Task<ActionResult<DynamicEntityDTO>> GetEntity(int moduleId, int entityId)
//        {
//            try
//            {
//                var entity = await _entityService.GetEntityByIdAsync(moduleId, entityId);
//                return Ok(entity);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Entity {EntityId} not found in module {ModuleId}: {Message}",
//                    entityId, moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving entity {EntityId} from module {ModuleId}",
//                    entityId, moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Creates a new entity
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityDto">Entity data</param>
//        /// <returns>Created entity</returns>
//        [HttpPost]
//        public async Task<ActionResult<DynamicEntityDTO>> CreateEntity(
//            int moduleId,
//            [FromBody] AddUpdateDynamicEntityDTO entityDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var createdEntity = await _entityService.CreateEntityAsync(moduleId, userId, entityDto);

//                return CreatedAtAction(
//                    nameof(GetEntity),
//                    new { moduleId, entityId = createdEntity.Id },
//                    createdEntity);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request creating entity in module {ModuleId}: {Message}",
//                    moduleId, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating entity in module {ModuleId}", moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Updates an existing entity
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityId">Entity ID</param>
//        /// <param name="entityDto">Updated entity data</param>
//        /// <returns>Updated entity</returns>
//        [HttpPut("{entityId}")]
//        public async Task<ActionResult<DynamicEntityDTO>> UpdateEntity(
//            int moduleId,
//            int entityId,
//            [FromBody] AddUpdateDynamicEntityDTO entityDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var updatedEntity = await _entityService.UpdateEntityAsync(moduleId, entityId, userId, entityDto);

//                return Ok(updatedEntity);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Entity {EntityId} not found in module {ModuleId}: {Message}",
//                    entityId, moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request updating entity {EntityId} in module {ModuleId}: {Message}",
//                    entityId, moduleId, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating entity {EntityId} in module {ModuleId}",
//                    entityId, moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Deletes an entity
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityId">Entity ID</param>
//        /// <returns>No content</returns>
//        [HttpDelete("{entityId}")]
//        public async Task<ActionResult> DeleteEntity(int moduleId, int entityId)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                await _entityService.DeleteEntityAsync(moduleId, entityId, userId);

//                return NoContent();
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Entity {EntityId} not found in module {ModuleId}: {Message}",
//                    entityId, moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request deleting entity {EntityId} in module {ModuleId}: {Message}",
//                    entityId, moduleId, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting entity {EntityId} from module {ModuleId}",
//                    entityId, moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets default values for creating a new entity
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <returns>Default values</returns>
//        [HttpGet("defaults")]
//        public async Task<ActionResult<Dictionary<string, object>>> GetDefaults(int moduleId)
//        {
//            try
//            {
//                var defaults = await _entityService.GetDefaultValuesAsync(moduleId);
//                return Ok(defaults);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error getting defaults for module {ModuleId}", moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Validates entity data without saving
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityDto">Entity data to validate</param>
//        /// <returns>Validation result</returns>
//        [HttpPost("validate")]
//        public async Task<ActionResult> ValidateEntity(
//            int moduleId,
//            [FromBody] AddUpdateDynamicEntityDTO entityDto)
//        {
//            try
//            {
//                await _entityService.ValidateEntityDataAsync(moduleId, entityDto.Data);
//                return Ok(new { isValid = true, message = "Entity data is valid" });
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", moduleId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                return BadRequest(new { isValid = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error validating entity for module {ModuleId}", moduleId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Validates entity data for update without saving
//        /// </summary>
//        /// <param name="moduleId">Module ID</param>
//        /// <param name="entityId">Entity ID</param>
//        /// <param name="entityDto">Entity data to validate</param>
//        /// <returns>Validation result</returns>
//        [HttpPost("{entityId}/validate")]
//        public async Task<ActionResult> ValidateEntityUpdate(
//            int moduleId,
//            int entityId,
//            [FromBody] AddUpdateDynamicEntityDTO entityDto)
//        {
//            try
//            {
//                await _entityService.ValidateEntityDataAsync(moduleId, entityDto.Data, entityId);
//                return Ok(new { isValid = true, message = "Entity data is valid" });
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} or Entity {EntityId} not found: {Message}",
//                    moduleId, entityId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                return BadRequest(new { isValid = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error validating entity update for module {ModuleId}, entity {EntityId}",
//                    moduleId, entityId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets current user ID from claims
//        /// </summary>
//        /// <returns>User ID</returns>
//        private int GetCurrentUserId()
//        {
//            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub") ?? User.FindFirst("userId");

//            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
//            {
//                return userId;
//            }

//            throw new UnauthorizedAccessException("User ID not found in token claims");
//        }
//    }

   
//}

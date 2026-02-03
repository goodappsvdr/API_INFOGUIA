//using Api.Infrastructure.Services.Dynamic;
//using Api.Shared.DTOs.Dynamic;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace API.Controllers
//{
//    /// <summary>
//    /// Controller for dynamic module management
//    /// </summary>
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize]
//    public class DynamicModulesController : ControllerBase
//    {
//        private readonly IDynamicModuleService _moduleService;
//        private readonly ILogger<DynamicModulesController> _logger;

//        public DynamicModulesController(
//            IDynamicModuleService moduleService,
//            ILogger<DynamicModulesController> logger)
//        {
//            _moduleService = moduleService;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Gets all dynamic modules
//        /// </summary>
//        /// <returns>List of modules</returns>
//        [HttpGet]
//        public async Task<ActionResult<List<DynamicModuleDTO>>> GetAllModules()
//        {
//            try
//            {
//                var modules = await _moduleService.GetAllModulesAsync();
//                return Ok(modules);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving all modules");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets all modules with statistics
//        /// </summary>
//        /// <returns>List of modules with stats</returns>
//        [HttpGet("stats")]
//        public async Task<ActionResult<List<DynamicModuleWithStatsDTO>>> GetAllModulesWithStats()
//        {
//            try
//            {
//                var modules = await _moduleService.GetModulesWithStatsAsync();
//                return Ok(modules);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving modules with stats");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets a specific module by ID
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <returns>Module details</returns>
//        [HttpGet("{id}")]
//        public async Task<ActionResult<DynamicModuleDTO>> GetModule(int id)
//        {
//            try
//            {
//                var module = await _moduleService.GetModuleByIdAsync(id);
//                return Ok(module);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", id, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving module {ModuleId}", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets a specific module with statistics
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <returns>Module with statistics</returns>
//        [HttpGet("{id}/stats")]
//        public async Task<ActionResult<DynamicModuleWithStatsDTO>> GetModuleWithStats(int id)
//        {
//            try
//            {
//                var module = await _moduleService.GetModuleWithStatsAsync(id);
//                return Ok(module);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", id, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving module {ModuleId} with stats", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Creates a new dynamic module
//        /// </summary>
//        /// <param name="moduleDto">Module data</param>
//        /// <returns>Created module</returns>
//        [HttpPost]
//        public async Task<ActionResult<DynamicModuleDTO>> CreateModule([FromBody] AddDynamicModuleDTO moduleDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var createdModule = await _moduleService.CreateModuleAsync(userId, moduleDto);

//                return CreatedAtAction(
//                    nameof(GetModule),
//                    new { id = createdModule.ModuleId },
//                    createdModule);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request creating module: {Message}", ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating module");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Updates an existing module
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <param name="moduleDto">Updated module data</param>
//        /// <returns>Updated module</returns>
//        [HttpPut("{id}")]
//        public async Task<ActionResult<DynamicModuleDTO>> UpdateModule(int id, [FromBody] UpdateDynamicModuleDTO moduleDto)
//        {
//            try
//            {
//                if (id != moduleDto.ModuleId)
//                {
//                    return BadRequest("Module ID in URL does not match module ID in body");
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var updatedModule = await _moduleService.UpdateModuleAsync(userId, moduleDto);

//                return Ok(updatedModule);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found for update: {Message}", id, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request updating module {ModuleId}: {Message}", id, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating module {ModuleId}", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Deletes a module
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <returns>No content</returns>
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteModule(int id)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                await _moduleService.DeleteModuleAsync(userId, id);

//                return NoContent();
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found for deletion: {Message}", id, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request deleting module {ModuleId}: {Message}", id, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting module {ModuleId}", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets all fields for a module
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <returns>List of fields</returns>
//        [HttpGet("{id}/fields")]
//        public async Task<ActionResult<List<DynamicFieldDTO>>> GetModuleFields(int id)
//        {
//            try
//            {
//                var fields = await _moduleService.GetModuleFieldsAsync(id);
//                return Ok(fields);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving fields for module {ModuleId}", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Adds a field to a module
//        /// </summary>
//        /// <param name="id">Module ID</param>
//        /// <param name="fieldDto">Field data</param>
//        /// <returns>Created field</returns>
//        [HttpPost("{id}/fields")]
//        public async Task<ActionResult<DynamicFieldDTO>> AddFieldToModule(int id, [FromBody] AddDynamicFieldDTO fieldDto)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var createdField = await _moduleService.AddFieldToModuleAsync(userId, id, fieldDto);

//                return CreatedAtAction(
//                    nameof(GetFieldById),
//                    new { fieldId = createdField.FieldId },
//                    createdField);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Module {ModuleId} not found: {Message}", id, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request adding field to module {ModuleId}: {Message}", id, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error adding field to module {ModuleId}", id);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Gets a specific field by ID
//        /// </summary>
//        /// <param name="fieldId">Field ID</param>
//        /// <returns>Field details</returns>
//        [HttpGet("fields/{fieldId}")]
//        public async Task<ActionResult<DynamicFieldDTO>> GetFieldById(int fieldId)
//        {
//            try
//            {
//                var field = await _moduleService.GetFieldByIdAsync(fieldId);
//                return Ok(field);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Field {FieldId} not found: {Message}", fieldId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving field {FieldId}", fieldId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Updates a field
//        /// </summary>
//        /// <param name="fieldId">Field ID</param>
//        /// <param name="fieldDto">Updated field data</param>
//        /// <returns>Updated field</returns>
//        [HttpPut("fields/{fieldId}")]
//        public async Task<ActionResult<DynamicFieldDTO>> UpdateField(int fieldId, [FromBody] UpdateDynamicFieldDTO fieldDto)
//        {
//            try
//            {
//                if (fieldId != fieldDto.FieldId)
//                {
//                    return BadRequest("Field ID in URL does not match field ID in body");
//                }

//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                var userId = GetCurrentUserId();
//                var updatedField = await _moduleService.UpdateFieldAsync(userId, fieldDto);

//                return Ok(updatedField);
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Field {FieldId} not found for update: {Message}", fieldId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request updating field {FieldId}: {Message}", fieldId, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating field {FieldId}", fieldId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Deletes a field
//        /// </summary>
//        /// <param name="fieldId">Field ID</param>
//        /// <returns>No content</returns>
//        [HttpDelete("fields/{fieldId}")]
//        public async Task<ActionResult> DeleteField(int fieldId)
//        {
//            try
//            {
//                var userId = GetCurrentUserId();
//                await _moduleService.DeleteFieldAsync(userId, fieldId);

//                return NoContent();
//            }
//            catch (NotFoundException ex)
//            {
//                _logger.LogWarning("Field {FieldId} not found for deletion: {Message}", fieldId, ex.Message);
//                return NotFound(ex.Message);
//            }
//            catch (BadRequestException ex)
//            {
//                _logger.LogWarning("Bad request deleting field {FieldId}: {Message}", fieldId, ex.Message);
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting field {FieldId}", fieldId);
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Validates module definition
//        /// </summary>
//        /// <param name="moduleDto">Module to validate</param>
//        /// <returns>Validation result</returns>
//        [HttpPost("validate")]
//        public async Task<ActionResult> ValidateModule([FromBody] AddDynamicModuleDTO moduleDto)
//        {
//            try
//            {
//                await _moduleService.ValidateModuleDefinitionAsync(moduleDto);
//                return Ok(new { isValid = true, message = "Module definition is valid" });
//            }
//            catch (BadRequestException ex)
//            {
//                return BadRequest(new { isValid = false, message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error validating module");
//                return StatusCode(500, "Internal server error");
//            }
//        }

//        /// <summary>
//        /// Checks if a table exists
//        /// </summary>
//        /// <param name="tableName">Table name</param>
//        /// <returns>Existence result</returns>
//        [HttpGet("tables/{tableName}/exists")]
//        public async Task<ActionResult<bool>> TableExists(string tableName)
//        {
//            try
//            {
//                var exists = await _moduleService.TableExistsAsync(tableName);
//                return Ok(exists);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error checking table existence for {TableName}", tableName);
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

//    /// <summary>
//    /// Exception classes para manejo de errores
//    /// </summary>
//    public class NotFoundException : Exception
//    {
//        public NotFoundException(string message) : base(message) { }
//    }

//    public class BadRequestException : Exception
//    {
//        public BadRequestException(string message) : base(message) { }
//    }

//}

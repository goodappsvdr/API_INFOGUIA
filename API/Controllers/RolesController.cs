using API.Extensions;
using API.Filters;
using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.Roles; 
using Api.Shared.DTOs;
using Api.Shared.DTOs.Roles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controlador de Roles
    /// </summary>
    [Route("api/Roles")]
    [ApiController]
    [JwtAuthorization]
    public class RolesController : ControllerBase
    {
        private readonly IRolesServices _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IRolesServices roleService,
            ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        // ===================== GET ALL ROLES =====================

        /// <summary>
        /// Obtiene la lista completa de roles disponibles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoleDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(ApiResponse<List<RoleDto>>.SuccessResponse(roles, "Roles retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all roles");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while fetching roles"));
            }
        }

        // ===================== GET ROLE BY ID =====================

        /// <summary>
        /// Obtiene un rol específico por su ID
        /// </summary>
        /// 
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                return Ok(ApiResponse<RoleDto>.SuccessResponse(role));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Role not found: {Id}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role {Id}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error retrieving role"));
            }
        }

        // ===================== CREATE ROLE =====================

        /// <summary>
        /// Crea un nuevo rol
        /// </summary>
        [HttpPost("CreateRole")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            if (dto == null) return BadRequest(ApiResponse<object>.ErrorResponse("Datos inválidos"));

            try
            {
                var created = await _roleService.CreateAsync(dto);

                // IMPORTANTE: Si el error 500 persiste aquí, asegúrate de tener un 
                // método llamado GetById(int id). Si no lo tienes, usa Ok() en su lugar.
                // Cambia esto en el Controller
                return Ok(ApiResponse<RoleDto>.SuccessResponse(created, "Role created successfully"));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear rol");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.InnerException?.Message ?? ex.Message));
            }

        }

        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        [HttpPut("UpdateRole/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos inválidos"));

            try
            {
                var updated = await _roleService.UpdateAsync(id, dto);

                return Ok(ApiResponse<RoleDto>.SuccessResponse(updated, "Role updated successfully"));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar rol");
                return StatusCode(500, ApiResponse<object>.ErrorResponse(ex.InnerException?.Message ?? ex.Message));
            }
        }


    }
}
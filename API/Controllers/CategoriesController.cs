using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.Categories;
using Api.Shared.DTOs;
using Api.Shared.DTOs.Categories;
using Api.Shared.Models;
using API.Extensions;
using API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[JwtAuthorization]
public class CategoriesController : ControllerBase
{
    private readonly ICategorieServices _categorieServices;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        ICategorieServices categorieServices,
        ILogger<CategoriesController> logger)
    {
        _categorieServices = categorieServices;
        _logger = logger;
    }

    /// <summary>
    /// Gets all categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CategorieDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCategories()
    {
        try
        {
            var categories = await _categorieServices.GetAllCategoriesAsync();
            return Ok(ApiResponse<List<CategorieDTO>>.SuccessResponse(
                categories,
                $"{categories.Count} category(ies) found"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while retrieving categories"));
        }
    }

    /// <summary>
    /// Gets categories with statistics
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryWithStatsDTO>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoriesWithStats()
    {
        try
        {
            var categories = await _categorieServices.GetCategoriesWithStatsAsync();
            return Ok(ApiResponse<List<CategoryWithStatsDTO>>.SuccessResponse(categories));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories with statistics");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while retrieving categories"));
        }
    }

    /// <summary>
    /// Gets a specific category by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategorieDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        try
        {
            var category = await _categorieServices.GetCategoryByIdAsync(id);
            return Ok(ApiResponse<CategorieDTO>.SuccessResponse(category));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category {CategoryId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while retrieving the category"));
        }
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategorieDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] AddCategoryDTO categoryDto)
    {
        try
        {
            var userId = int.Parse(HttpContext.GetUserId());
            var createdCategory = await _categorieServices.CreateCategoryAsync(userId, categoryDto);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = createdCategory.Id },
                ApiResponse<CategorieDTO>.SuccessResponse(
                    createdCategory,
                    "Category created successfully"));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while creating the category"));
        }
    }

    /// <summary>
    /// Updates an existing category
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategorieDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO categoryDto)
    {
        try
        {
            if (id != categoryDto.Id)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "ID in URL doesn't match ID in body"));
            }

            var userId = int.Parse(HttpContext.GetUserId());
            var updatedCategory = await _categorieServices.UpdateCategoryAsync(userId, categoryDto);

            return Ok(ApiResponse<CategorieDTO>.SuccessResponse(
                updatedCategory,
                "Category updated successfully"));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category {CategoryId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while updating the category"));
        }
    }

    /// <summary>
    /// Deletes a category
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            var userId = int.Parse(HttpContext.GetUserId());
            await _categorieServices.DeleteCategoryAsync(userId, id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category {CategoryId}", id);
            return StatusCode(500, ApiResponse<object>.ErrorResponse(
                "An error occurred while deleting the category"));
        }
    }
}
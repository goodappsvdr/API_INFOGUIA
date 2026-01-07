using Api.Infrastructure.Exceptions;
using Api.Shared.Data;
using Api.Shared.DTOs.Categories;
using Api.Shared.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.Categories;

public class CategorieServices : ICategorieServices
{
    private readonly IMapper _mapper;
    private readonly ContextInfoGuia _context;
    private readonly ILogger<CategorieServices> _logger;

    public CategorieServices(
        IMapper mapper,
        ContextInfoGuia context,
        ILogger<CategorieServices> logger)
    {
        _mapper = mapper;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all categories
    /// </summary>
    /// <returns>List of categories</returns>
    public async Task<List<CategorieDTO>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} categories", categories.Count);

            return _mapper.Map<List<CategorieDTO>>(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all categories");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a category by its ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>Category details</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found</exception>
    public async Task<CategorieDTO> GetCategoryByIdAsync(int id)
    {
        try
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", id);
                throw new NotFoundException($"Category with ID {id} not found");
            }

            return _mapper.Map<CategorieDTO>(category);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category {CategoryId}", id);
            throw;
        }
    }

    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="userId">User ID creating the category</param>
    /// <param name="categoryDto">Category data</param>
    /// <returns>Created category</returns>
    /// <exception cref="BadRequestException">Thrown when category data is invalid</exception>
    public async Task<CategorieDTO> CreateCategoryAsync(int userId, AddCategoryDTO categoryDto)
    {
        try
        {
            // Validar que no exista una categoría con el mismo nombre
            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower());

            if (existingCategory != null)
            {
                throw new BadRequestException($"A category with the name '{categoryDto.Name}' already exists");
            }

            var category = _mapper.Map<Category>(categoryDto);
            category.CreatedAt = DateTime.UtcNow;
            category.CreatedByUserId = userId;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category '{CategoryName}' with ID {CategoryId} created by user {UserId}",
                category.Name,
                category.CategoryId,
                userId);

            return _mapper.Map<CategorieDTO>(category);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category for user {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing category
    /// </summary>
    /// <param name="userId">User ID performing the update</param>
    /// <param name="categoryDto">Updated category data</param>
    /// <returns>Updated category</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found</exception>
    /// <exception cref="BadRequestException">Thrown when update data is invalid</exception>
    public async Task<CategorieDTO> UpdateCategoryAsync(int userId, UpdateCategoryDTO categoryDto)
    {
        try
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == categoryDto.Id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found for update", categoryDto.Id);
                throw new NotFoundException($"Category with ID {categoryDto.Id} not found");
            }

            // Validar que no exista otra categoría con el mismo nombre
            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.Name.ToLower() == categoryDto.Name.ToLower() &&
                    c.CategoryId != categoryDto.Id);

            if (existingCategory != null)
            {
                throw new BadRequestException($"A category with the name '{categoryDto.Name}' already exists");
            }

            _mapper.Map(categoryDto, category);
            category.ModifiedAt = DateTime.UtcNow;
            category.ModifiedByUserId = userId;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category '{CategoryName}' with ID {CategoryId} updated by user {UserId}",
                category.Name,
                category.CategoryId,
                userId);

            return _mapper.Map<CategorieDTO>(category);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category {CategoryId}", categoryDto.Id);
            throw;
        }
    }

    /// <summary>
    /// Deletes a category
    /// </summary>
    /// <param name="userId">User ID performing the deletion</param>
    /// <param name="id">Category ID to delete</param>
    /// <exception cref="NotFoundException">Thrown when category is not found</exception>
    /// <exception cref="BadRequestException">Thrown when category cannot be deleted</exception>
    public async Task DeleteCategoryAsync(int userId, int id)
    {
        try
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryId == id);

            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found for deletion", id);
                throw new NotFoundException($"Category with ID {id} not found");
            }

            // Verificar si la categoría tiene listings asociados
            var hasListings = await _context.Listings
                .AnyAsync(l => l.CategoryId == id);

            if (hasListings)
            {
                throw new BadRequestException(
                    "Cannot delete category because it has associated listings. " +
                    "Please reassign or delete the listings first.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Category '{CategoryName}' with ID {CategoryId} deleted by user {UserId}",
                category.Name,
                id,
                userId);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category {CategoryId}", id);
            throw;
        }
    }

    /// <summary>
    /// Gets categories with their listing count
    /// </summary>
    /// <returns>List of categories with statistics</returns>
    public async Task<List<CategoryWithStatsDTO>> GetCategoriesWithStatsAsync()
    {
        try
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryWithStatsDTO
                {
                    Id = c.CategoryId,
                    Name = c.Name,
                    Description = c.Name,
                    ListingCount = _context.Listings.Count(l => l.CategoryId == c.CategoryId),
                    CreatedAt = c.CreatedAt
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} categories with statistics", categories.Count);

            return categories;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories with statistics");
            throw;
        }
    }
}
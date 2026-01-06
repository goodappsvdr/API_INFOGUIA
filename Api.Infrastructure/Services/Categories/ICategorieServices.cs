using Api.Shared.DTOs.Categories;

namespace Api.Infrastructure.Services.Categories
{
    public interface ICategorieServices
    {
        /// <summary>
        /// Retrieves all categories
        /// </summary>
        Task<List<CategorieDTO>> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category by its ID
        /// </summary>
        Task<CategorieDTO> GetCategoryByIdAsync(int id);

        /// <summary>
        /// Creates a new category
        /// </summary>
        Task<CategorieDTO> CreateCategoryAsync(int userId, AddCategoryDTO categoryDto);

        /// <summary>
        /// Updates an existing category
        /// </summary>
        Task<CategorieDTO> UpdateCategoryAsync(int userId, UpdateCategoryDTO categoryDto);

        /// <summary>
        /// Deletes a category
        /// </summary>
        Task DeleteCategoryAsync(int userId, int id);

        /// <summary>
        /// Gets categories with their listing count
        /// </summary>
        Task<List<CategoryWithStatsDTO>> GetCategoriesWithStatsAsync();
    }
}
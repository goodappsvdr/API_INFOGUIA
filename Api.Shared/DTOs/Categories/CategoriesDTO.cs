using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Categories
{
    public class CategorieDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class AddCategoryDTO
    {
        public string Name { get; set; }
        public string IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }
        public int SortOrder { get; set; }


    }

    public class UpdateCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public string IconUrl { get; set; }
        public int SortOrder { get; set; }

        public bool IsActive { get; set; }  
    }
    public class CategoryWithStatsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ListingCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

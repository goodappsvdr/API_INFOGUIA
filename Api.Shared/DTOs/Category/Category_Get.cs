namespace Api.Shared.DTOs.Category
{
    public class Category_Get
    {
        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public bool? Active { get; set; }
    }

    public class Category_Create
    {
        public string? Description { get; set; }

        public bool? Active { get; set; }
    }
    public class Category_Update
    {
        public int CategoryId { get; set; }

        public string? Description { get; set; }

        public bool? Active { get; set; }
    }
}

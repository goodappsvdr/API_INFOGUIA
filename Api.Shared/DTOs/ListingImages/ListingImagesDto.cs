namespace Api.Shared.DTOs.ListingImages
{
    public class ListingImagesDto
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddListingImagesDTO
    {
        public int ListingId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }
        public int SortOrder { get; set; }
    }

    public class UpdateListingImagesDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetListingImages
    {
        public int ListingImageID { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string? Caption { get; set; }
    }

}

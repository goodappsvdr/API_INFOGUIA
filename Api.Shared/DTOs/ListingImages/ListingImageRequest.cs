
using Microsoft.AspNetCore.Http;

namespace Api.Shared.DTOs.ListingImages
{

    public class ListingImageUrlRequest
    {
        public int listingId { get; set; }
        public string imageUrl { get; set; }
        public string caption { get; set; }
        public int userId { get; set; }
    }


    public class GetUrlListingImageUrl
    {
        public string imageUrl { get; set; }
    }


    public class ListingImageUploadRequest
    {
        public int ListingId { get; set; }
        public IFormFile ImageFile { get; set; } // El selector de archivos en Swagger
        public string? Caption { get; set; }
        public int UserId { get; set; }
    }
}
using Microsoft.AspNetCore.Http;

namespace Api.Shared.DTOs.Identity.AspNetUsers
{
    public class User_UpdateImage
    {
        public IFormFile Image { get; set; } = default!;
    }
}

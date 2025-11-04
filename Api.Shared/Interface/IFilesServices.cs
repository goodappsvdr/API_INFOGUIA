namespace Api.Shared.Interface
{
    public interface IFilesServices
    {
        Task<string> DeleteImage(string UrlImage);
        Task<string> UploadImage(IFormFile File, string ControllerName);
        Task<string> UploadImageWebp(IFormFile File, string ControllerName);
    }
}
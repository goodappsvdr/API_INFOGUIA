using Microsoft.AspNetCore.Hosting;

namespace Api.Infrastructure.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FilesServices(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        // Combina la direccion base con el nombre del archivo
        private async Task<string> GetFilePatch(string FileName)
        {
            string BadeDir = _webHostEnvironment.WebRootPath;
            var FilePath = Path.Combine(BadeDir, FileName);
            return FilePath.Replace("\\", "/");
        }

        // Combina la direccion web con el nombre del archivo
        private async Task<string> GetUrlFile(string FilePath)
        {
            string UrlFile = _httpContextAccessor.HttpContext.Request.Host.ToUriComponent();
            FilePath = FilePath.Replace("\\", "/");
            return $"https://{UrlFile}/{FilePath}";
        }

        // Carga la imagen en formato webp
        public async Task<string> UploadImageWebp(IFormFile File, string ControllerName)
        {
            try
            {
                string FileName = $"{ControllerName}/{Guid.NewGuid()}{".webp"}";
                string FilePath = await GetFilePatch(FileName);
                using (var memoryStream = new MemoryStream())
                {
                    await File.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using (Image<Rgba32> image = Image.Load<Rgba32>(memoryStream))
                    {
                        image.Save(FilePath, new JpegEncoder());
                    }
                }
                return await GetUrlFile(FileName);
            }
            catch (Exception)
            {
                return "";
            }

        }

        // carga la imagen en el mismo formato
        public async Task<string> UploadImage(IFormFile File, string ControllerName)
        {
            try
            {
                string FileName = $"Images/{ControllerName}/{Guid.NewGuid()}{Path.GetExtension(File.FileName)}";
                string FilePath = await GetFilePatch(FileName);

                using (var memoryStream = new MemoryStream())
                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    await File.CopyToAsync(fileStream);
                }
                return await GetUrlFile(FileName);
            }
            catch (Exception)
            {
                return "";
            }
        }

        // elimina la imagen en el servidor
        public async Task<string> DeleteImage(string UrlImage)
        {
            try
            {
                string FileName = UrlImage.Replace($"https://{_httpContextAccessor.HttpContext.Request.Host.ToUriComponent()}", "");
                string BadeDir = _webHostEnvironment.WebRootPath;
                var FilePath = $"{BadeDir}\\{FileName}";
                if (File.Exists(FilePath)) File.Delete(FilePath);
                return await GetUrlFile(FileName);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

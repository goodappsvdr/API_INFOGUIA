namespace API.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.Items["UserId"]?.ToString();
        }
    }
}

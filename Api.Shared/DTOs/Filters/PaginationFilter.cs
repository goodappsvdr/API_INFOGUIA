namespace Api.Shared.DTOs.Filters
{
    public class PaginationFilter
    {
        public int PageSize { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}

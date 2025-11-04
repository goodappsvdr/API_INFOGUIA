namespace Api.Shared.DTOs.Filters
{
    public class ResponseFilter<T>
    {
        public ResponseFilter(List<T> data, int count, int offset, int pageSize)
        {
            Data = data;
            Offset = offset;
            PageSize = pageSize;
            NextOffset = offset + pageSize;
            TotalCount = count;
        }
        public List<T> Data { get; set; }

        public int Offset { get; set; }

        public int NextOffset { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

    }



}

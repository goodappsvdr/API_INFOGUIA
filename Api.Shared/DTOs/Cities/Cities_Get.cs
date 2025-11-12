namespace Api.Shared.DTOs.Cities
{
    public class Cities_Get
    {
        public long CityId { get; set; }
        public int ProvinceId { get; set; }
        public string? Name { get; set; }
    }
}

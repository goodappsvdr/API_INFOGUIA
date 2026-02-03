namespace Api.Shared.DTOs.ListingHours
{
    public class ListingHoursDto
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public int DayOfWeek { get; set; }   // 0-6 o 1-7 según tu criterio
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddListingHoursDTO
    {
        public int ListingId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
    }

    public class UpdateListingHoursDTO
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidUntil { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetListingHours
    {
        public int DayOfWeek { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }

}

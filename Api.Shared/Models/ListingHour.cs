using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingHour
{
    public int ListingHourId { get; set; }

    public int ListingId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly? OpenTime { get; set; }

    public TimeOnly? CloseTime { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidUntil { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}

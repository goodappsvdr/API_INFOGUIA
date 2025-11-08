using Api.Shared.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingService : BaseEntity
{
    public int ListingId { get; set; }

    public int ServiceId { get; set; }
}

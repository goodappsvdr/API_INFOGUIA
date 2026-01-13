using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingUser
{
    public int ListingUserId { get; set; }

    public int? ListingId { get; set; }

    public int? UserId { get; set; }
}

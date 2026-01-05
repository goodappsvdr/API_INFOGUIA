using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingPaymentMethod
{
    public int ListingId { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}

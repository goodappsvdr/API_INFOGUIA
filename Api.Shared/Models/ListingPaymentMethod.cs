using Api.Shared.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingPaymentMethod : BaseEntity
{
    public int ListingId { get; set; }

    public int PaymentMethodId { get; set; }
}

using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public int TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }
}

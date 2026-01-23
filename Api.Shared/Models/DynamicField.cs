using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class DynamicField
{
    public int FieldId { get; set; }

    public int ModuleId { get; set; }

    public string FieldName { get; set; } = null!;

    public string ColumnName { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public int? MaxLength { get; set; }

    public int? Precision { get; set; }

    public int? Scale { get; set; }

    public bool IsRequired { get; set; }

    public bool IsUnique { get; set; }

    public bool IsPrimaryKey { get; set; }

    public bool IsIdentity { get; set; }

    public string? DefaultValue { get; set; }

    public string? DisplayName { get; set; }

    public string? Description { get; set; }

    public bool ShowInList { get; set; }

    public bool ShowInForm { get; set; }

    public int SortOrder { get; set; }

    public string? LookupTable { get; set; }

    public string? LookupKeyColumn { get; set; }

    public string? LookupDisplayColumn { get; set; }

    public string? ValidationRules { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Api.Shared.DTOs.Dynamic;

/// <summary>
/// DTO para mostrar información básica del módulo
/// </summary>
public class DynamicModuleDTO
{
    public int ModuleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TableName { get; set; }
    public string EntityName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<DynamicFieldDTO> Fields { get; set; } = new List<DynamicFieldDTO>();
}

/// <summary>
/// DTO para crear un nuevo módulo
/// </summary>
public class AddDynamicModuleDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]*$", ErrorMessage = "Table name must start with a letter and contain only letters, numbers, and underscores")]
    public string TableName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9]*$", ErrorMessage = "Entity name must start with a letter and contain only letters and numbers")]
    public string EntityName { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one field is required")]
    public List<AddDynamicFieldDTO> Fields { get; set; } = new List<AddDynamicFieldDTO>();
}

/// <summary>
/// DTO para actualizar un módulo existente
/// </summary>
public class UpdateDynamicModuleDTO
{
    [Required]
    public int ModuleId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; }

    public List<UpdateDynamicFieldDTO> Fields { get; set; } = new List<UpdateDynamicFieldDTO>();
}

/// <summary>
/// DTO para mostrar información del campo
/// </summary>
public class DynamicFieldDTO
{
    public int FieldId { get; set; }
    public int ModuleId { get; set; }
    public string Name { get; set; }
    public string ColumnName { get; set; }
    public string DataType { get; set; }
    public int? MaxLength { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public bool IsRequired { get; set; }
    public bool IsUnique { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsIdentity { get; set; }
    public string DefaultValue { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
    public bool ShowInList { get; set; }
    public bool ShowInForm { get; set; }
    public string LookupTable { get; set; }
    public string LookupKeyColumn { get; set; }
    public string LookupDisplayColumn { get; set; }
    public string ValidationRules { get; set; }
}

/// <summary>
/// DTO para agregar un nuevo campo
/// </summary>
public class AddDynamicFieldDTO
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@"^[A-Za-z][A-Za-z0-9_]*$", ErrorMessage = "Column name must start with a letter and contain only letters, numbers, and underscores")]
    public string ColumnName { get; set; }

    [Required]
    [StringLength(50)]
    public string DataType { get; set; }

    public int? MaxLength { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public bool IsRequired { get; set; }
    public bool IsUnique { get; set; }
    public bool IsPrimaryKey { get; set; }
    public bool IsIdentity { get; set; }
    public string DefaultValue { get; set; }

    [StringLength(200)]
    public string DisplayName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public int SortOrder { get; set; }
    public bool ShowInList { get; set; } = true;
    public bool ShowInForm { get; set; } = true;

    // Para lookups
    public string LookupTable { get; set; }
    public string LookupKeyColumn { get; set; }
    public string LookupDisplayColumn { get; set; }

    public string ValidationRules { get; set; }
}

/// <summary>
/// DTO para actualizar un campo existente
/// </summary>
public class UpdateDynamicFieldDTO : AddDynamicFieldDTO
{
    public int FieldId { get; set; }
}

/// <summary>
/// DTO para mostrar módulos con estadísticas
/// </summary>
public class DynamicModuleWithStatsDTO
{
    public int ModuleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TableName { get; set; }
    public string EntityName { get; set; }
    public bool IsActive { get; set; }
    public int FieldCount { get; set; }
    public int RecordCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

/// <summary>
/// DTO para operaciones CRUD dinámicas
/// </summary>
public class DynamicEntityDTO
{
    public int Id { get; set; }
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    public DateTime CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public int? ModifiedByUserId { get; set; }
}

/// <summary>
/// DTO para crear/actualizar entidades dinámicas
/// </summary>
public class AddUpdateDynamicEntityDTO
{
    public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// DTO para respuesta de listado con paginación
/// </summary>
public class DynamicEntityListResponseDTO
{
    public List<DynamicEntityDTO> Data { get; set; } = new List<DynamicEntityDTO>();
    public int TotalRecords { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

/// <summary>
/// DTO para parámetros de búsqueda y filtrado
/// </summary>
public class DynamicEntitySearchDTO
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SearchTerm { get; set; }
    public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();
    public string SortField { get; set; }
    public bool SortAscending { get; set; } = true;
}
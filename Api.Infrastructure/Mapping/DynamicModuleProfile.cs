using Api.Shared.DTOs.Dynamic;
using AutoMapper;

namespace Api.Infrastructure.Mapping;

/// <summary>
/// AutoMapper profile for dynamic modules
/// </summary>
public class DynamicModuleProfile : Profile
{
    public DynamicModuleProfile()
    {
        // Dynamic Module mappings
        CreateMap<DynamicModule, DynamicModuleDTO>()
            .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.DynamicFields.OrderBy(f => f.SortOrder)));

        CreateMap<AddDynamicModuleDTO, DynamicModule>()
            .ForMember(dest => dest.ModuleId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.DynamicFields, opt => opt.MapFrom(src => src.Fields));

        CreateMap<UpdateDynamicModuleDTO, DynamicModule>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.TableName, opt => opt.Ignore()) // No permitir cambiar nombre de tabla
            .ForMember(dest => dest.EntityName, opt => opt.Ignore()) // No permitir cambiar nombre de entidad
            .ForMember(dest => dest.DynamicFields, opt => opt.Ignore()); // Los campos se manejan por separado

        // Dynamic Field mappings
        CreateMap<DynamicField, DynamicFieldDTO>();

        CreateMap<AddDynamicFieldDTO, DynamicField>()
            .ForMember(dest => dest.FieldId, opt => opt.Ignore())
            .ForMember(dest => dest.ModuleId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.SortOrder, opt => opt.Ignore()) // Se asigna en el servicio
            .ForMember(dest => dest.Module, opt => opt.Ignore());

        CreateMap<UpdateDynamicFieldDTO, DynamicField>()
            .ForMember(dest => dest.ModuleId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.Module, opt => opt.Ignore())
            // Campos que no se pueden cambiar en una actualización
            .ForMember(dest => dest.ColumnName, opt => opt.Ignore())
            .ForMember(dest => dest.DataType, opt => opt.Ignore())
            .ForMember(dest => dest.MaxLength, opt => opt.Ignore())
            .ForMember(dest => dest.Precision, opt => opt.Ignore())
            .ForMember(dest => dest.Scale, opt => opt.Ignore())
            .ForMember(dest => dest.IsRequired, opt => opt.Ignore())
            .ForMember(dest => dest.IsUnique, opt => opt.Ignore())
            .ForMember(dest => dest.IsPrimaryKey, opt => opt.Ignore())
            .ForMember(dest => dest.IsIdentity, opt => opt.Ignore())
            .ForMember(dest => dest.DefaultValue, opt => opt.Ignore());

        // Mapping from DTO to update existing entity (para el método Map(source, destination))
        CreateMap<UpdateDynamicModuleDTO, DynamicModule>()
            .ForMember(dest => dest.ModuleId, opt => opt.Ignore())
            .ForMember(dest => dest.TableName, opt => opt.Ignore())
            .ForMember(dest => dest.EntityName, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.DynamicFields, opt => opt.Ignore());

        CreateMap<UpdateDynamicFieldDTO, DynamicField>()
            .ForMember(dest => dest.FieldId, opt => opt.Ignore())
            .ForMember(dest => dest.ModuleId, opt => opt.Ignore())
            .ForMember(dest => dest.ColumnName, opt => opt.Ignore())
            .ForMember(dest => dest.DataType, opt => opt.Ignore())
            .ForMember(dest => dest.MaxLength, opt => opt.Ignore())
            .ForMember(dest => dest.Precision, opt => opt.Ignore())
            .ForMember(dest => dest.Scale, opt => opt.Ignore())
            .ForMember(dest => dest.IsRequired, opt => opt.Ignore())
            .ForMember(dest => dest.IsUnique, opt => opt.Ignore())
            .ForMember(dest => dest.IsPrimaryKey, opt => opt.Ignore())
            .ForMember(dest => dest.IsIdentity, opt => opt.Ignore())
            .ForMember(dest => dest.DefaultValue, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.Module, opt => opt.Ignore());
    }
}

/// <summary>
/// Extension methods for AutoMapper
/// </summary>
public static class DynamicMappingExtensions
{
    /// <summary>
    /// Maps a collection of AddDynamicFieldDTO to DynamicField with module ID
    /// </summary>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="fields">Field DTOs</param>
    /// <param name="moduleId">Module ID</param>
    /// <returns>Mapped fields</returns>
    public static ICollection<DynamicField> MapFields(this IMapper mapper, ICollection<AddDynamicFieldDTO> fields, int moduleId)
    {
        var mappedFields = new List<DynamicField>();

        foreach (var fieldDto in fields)
        {
            var field = mapper.Map<DynamicField>(fieldDto);
            field.ModuleId = moduleId;
            mappedFields.Add(field);
        }

        return mappedFields;
    }

    /// <summary>
    /// Maps a collection of UpdateDynamicFieldDTO to existing DynamicField entities
    /// </summary>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="fieldDtos">Field DTOs</param>
    /// <param name="existingFields">Existing field entities</param>
    /// <returns>Updated fields</returns>
    public static ICollection<DynamicField> MapFieldUpdates(this IMapper mapper,
        ICollection<UpdateDynamicFieldDTO> fieldDtos,
        ICollection<DynamicField> existingFields)
    {
        var updatedFields = new List<DynamicField>();

        foreach (var fieldDto in fieldDtos)
        {
            var existingField = existingFields.FirstOrDefault(f => f.FieldId == fieldDto.FieldId);
            if (existingField != null)
            {
                mapper.Map(fieldDto, existingField);
                updatedFields.Add(existingField);
            }
        }

        return updatedFields;
    }

    /// <summary>
    /// Creates a safe copy of a dynamic module for response
    /// </summary>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="module">Source module</param>
    /// <returns>Safe module DTO</returns>
    public static DynamicModuleDTO ToSafeDto(this IMapper mapper, DynamicModule module)
    {
        var dto = mapper.Map<DynamicModuleDTO>(module);

        // Asegurar que los campos estén ordenados
        dto.Fields = dto.Fields.OrderBy(f => f.SortOrder).ToList();

        return dto;
    }

    /// <summary>
    /// Validates field mapping consistency
    /// </summary>
    /// <param name="fields">Fields to validate</param>
    /// <returns>Validation errors</returns>
    public static List<string> ValidateFieldMappings(this ICollection<DynamicFieldDTO> fields)
    {
        var errors = new List<string>();

        // Verificar que hay al menos una PK
        if (!fields.Any(f => f.IsPrimaryKey))
        {
            errors.Add("At least one field must be marked as Primary Key");
        }

        // Verificar que los nombres de columna son únicos
        var duplicateColumns = fields
            .GroupBy(f => f.ColumnName.ToLower())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateColumns.Any())
        {
            errors.Add($"Duplicate column names found: {string.Join(", ", duplicateColumns)}");
        }

        // Verificar que solo hay un campo identity
        var identityFields = fields.Where(f => f.IsIdentity).ToList();
        if (identityFields.Count > 1)
        {
            errors.Add("Only one field can be marked as Identity");
        }

        // Verificar que el campo identity es también PK
        var identityField = identityFields.FirstOrDefault();
        if (identityField != null && !identityField.IsPrimaryKey)
        {
            errors.Add("Identity field must also be marked as Primary Key");
        }

        return errors;
    }
}

/// <summary>
/// Custom value resolver for field sort orders
/// </summary>
public class FieldSortOrderResolver : IValueResolver<AddDynamicModuleDTO, DynamicModule, ICollection<DynamicField>>
{
    private readonly IMapper _mapper;

    public FieldSortOrderResolver(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ICollection<DynamicField> Resolve(AddDynamicModuleDTO source, DynamicModule destination,
        ICollection<DynamicField> destMember, ResolutionContext context)
    {
        var fields = new List<DynamicField>();

        for (int i = 0; i < source.Fields.Count; i++)
        {
            var field = _mapper.Map<DynamicField>(source.Fields.ElementAt(i));
            field.SortOrder = i + 1; // Asignar orden basado en la posición
            fields.Add(field);
        }

        return fields;
    }
}

/// <summary>
/// Custom condition for conditional mapping
/// </summary>
public static class DynamicMappingConditions
{
    /// <summary>
    /// Condition to only map non-null and non-empty strings
    /// </summary>
    /// <param name="src">Source value</param>
    /// <param name="dest">Destination value</param>
    /// <param name="srcMember">Source member value</param>
    /// <param name="destMember">Destination member</param>
    /// <param name="context">Resolution context</param>
    /// <returns>True if should map</returns>
    public static bool MapIfNotNullOrEmpty(object src, object dest, object srcMember, object destMember, ResolutionContext context)
    {
        return srcMember != null && !string.IsNullOrEmpty(srcMember.ToString());
    }

    /// <summary>
    /// Condition to only map if value is different
    /// </summary>
    /// <param name="src">Source value</param>
    /// <param name="dest">Destination value</param>
    /// <param name="srcMember">Source member value</param>
    /// <param name="destMember">Destination member</param>
    /// <param name="context">Resolution context</param>
    /// <returns>True if should map</returns>
    public static bool MapIfDifferent(object src, object dest, object srcMember, object destMember, ResolutionContext context)
    {
        return !Equals(srcMember, destMember);
    }
}
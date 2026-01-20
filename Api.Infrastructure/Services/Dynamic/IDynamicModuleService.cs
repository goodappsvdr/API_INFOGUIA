using Api.Shared.DTOs.Dynamic;

namespace Api.Infrastructure.Services.Dynamic;

/// <summary>
/// Interfaz para el servicio de módulos dinámicos
/// </summary>
public interface IDynamicModuleService
{
    // Gestión de módulos
    Task<List<DynamicModuleDTO>> GetAllModulesAsync();
    Task<DynamicModuleDTO> GetModuleByIdAsync(int moduleId);
    Task<DynamicModuleWithStatsDTO> GetModuleWithStatsAsync(int moduleId);
    Task<List<DynamicModuleWithStatsDTO>> GetModulesWithStatsAsync();
    Task<DynamicModuleDTO> CreateModuleAsync(int userId, AddDynamicModuleDTO moduleDto);
    Task<DynamicModuleDTO> UpdateModuleAsync(int userId, UpdateDynamicModuleDTO moduleDto);
    Task DeleteModuleAsync(int userId, int moduleId);
    Task<bool> TableExistsAsync(string tableName);
    Task<bool> ModuleExistsAsync(string name, int? excludeId = null);

    // Gestión de campos
    Task<List<DynamicFieldDTO>> GetModuleFieldsAsync(int moduleId);
    Task<DynamicFieldDTO> GetFieldByIdAsync(int fieldId);
    Task<DynamicFieldDTO> AddFieldToModuleAsync(int userId, int moduleId, AddDynamicFieldDTO fieldDto);
    Task<DynamicFieldDTO> UpdateFieldAsync(int userId, UpdateDynamicFieldDTO fieldDto);
    Task DeleteFieldAsync(int userId, int fieldId);

    // Validaciones
    Task ValidateModuleDefinitionAsync(AddDynamicModuleDTO moduleDto);
    Task ValidateFieldDefinitionAsync(AddDynamicFieldDTO fieldDto, int? moduleId = null);
    Task ValidateModuleUpdateAsync(UpdateDynamicModuleDTO moduleDto);
}

/// <summary>
/// Interfaz para el servicio de entidades dinámicas (CRUD)
/// </summary>
public interface IDynamicEntityService
{
    // CRUD de entidades dinámicas
    Task<DynamicEntityListResponseDTO> GetEntitiesAsync(int moduleId, DynamicEntitySearchDTO searchDto);
    Task<DynamicEntityDTO> GetEntityByIdAsync(int moduleId, int entityId);
    Task<DynamicEntityDTO> CreateEntityAsync(int moduleId, int userId, AddUpdateDynamicEntityDTO entityDto);
    Task<DynamicEntityDTO> UpdateEntityAsync(int moduleId, int entityId, int userId, AddUpdateDynamicEntityDTO entityDto);
    Task DeleteEntityAsync(int moduleId, int entityId, int userId);

    // Validaciones dinámicas
    Task ValidateEntityDataAsync(int moduleId, Dictionary<string, object> data, int? excludeEntityId = null);

    // Utilidades
    Task<Dictionary<string, object>> GetDefaultValuesAsync(int moduleId);
    Task<List<dynamic>> GetLookupDataAsync(string tableName, string keyColumn, string displayColumn, string searchTerm = null);
}

/// <summary>
/// Interfaz para el servicio de generación de base de datos
/// </summary>
public interface IDynamicDatabaseService
{
    // Generación de esquema
    Task CreateTableAsync(DynamicModuleDTO moduleDto);
    Task DropTableAsync(string tableName);
    Task AlterTableAddColumnAsync(string tableName, DynamicFieldDTO fieldDto);
    Task AlterTableDropColumnAsync(string tableName, string columnName);
    Task AlterTableModifyColumnAsync(string tableName, DynamicFieldDTO fieldDto);

    // Utilidades de base de datos
    Task<bool> TableExistsAsync(string tableName);
    Task<bool> ColumnExistsAsync(string tableName, string columnName);
    Task<List<string>> GetTableColumnsAsync(string tableName);
    Task<string> GenerateCreateTableScriptAsync(DynamicModuleDTO moduleDto);
    Task<string> GenerateAlterTableScriptAsync(string tableName, DynamicFieldDTO fieldDto, string operation);

    // Validaciones de esquema
    Task ValidateTableStructureAsync(DynamicModuleDTO moduleDto);
    Task<bool> CanDropColumnAsync(string tableName, string columnName);
}

/// <summary>
/// Interfaz para el servicio de AutoMapper dinámico
/// </summary>
public interface IDynamicMappingService
{
    // Mapeo dinámico
    DynamicEntityDTO MapToDynamicEntity(Dictionary<string, object> dbData, List<DynamicFieldDTO> fields);
    Dictionary<string, object> MapFromDynamicEntity(AddUpdateDynamicEntityDTO entityDto, List<DynamicFieldDTO> fields);

    // Validaciones de tipos
    object ConvertValue(object value, DynamicFieldDTO field);
    bool IsValidValue(object value, DynamicFieldDTO field);

    // Utilidades
    Type GetClrType(string sqlDataType);
    string GetSqlDataType(Type clrType, int? maxLength = null, int? precision = null, int? scale = null);
}
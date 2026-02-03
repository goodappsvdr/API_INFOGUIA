using Api.Infrastructure.Exceptions;
using Api.Infrastructure.Services.Dynamic;
using Api.Shared.Data;
using Api.Shared.DTOs.Dynamic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using System.Text.Json;

namespace Api.Infrastructure.Services.Dynamic;

public class DynamicEntityService : IDynamicEntityService
{
    private readonly Context _context;
    private readonly ILogger<DynamicEntityService> _logger;
    private readonly IDynamicModuleService _moduleService;
    private readonly IDynamicMappingService _mappingService;

    public DynamicEntityService(
        Context context,
        ILogger<DynamicEntityService> logger,
        IDynamicModuleService moduleService,
        IDynamicMappingService mappingService)
    {
        _context = context;
        _logger = logger;
        _moduleService = moduleService;
        _mappingService = mappingService;
    }

    /// <summary>
    /// Gets entities with pagination and filtering
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="searchDto">Search parameters</param>
    /// <returns>Paginated entity list</returns>
    public async Task<DynamicEntityListResponseDTO> GetEntitiesAsync(int moduleId, DynamicEntitySearchDTO searchDto)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);

            // Construir query dinámico
            var queryBuilder = new StringBuilder();
            var parameters = new List<SqlParameter>();

            // SELECT
            queryBuilder.Append("SELECT ");
            var selectColumns = fields.Where(f => f.ShowInList).Select(f => $"[{f.ColumnName}]").ToList();
            selectColumns.Add("[CreatedAt]");
            selectColumns.Add("[ModifiedAt]");
            selectColumns.Add("[CreatedByUserId]");
            selectColumns.Add("[ModifiedByUserId]");

            queryBuilder.AppendLine(string.Join(", ", selectColumns));
            queryBuilder.AppendLine($"FROM [{module.TableName}]");

            // WHERE
            var whereConditions = new List<string>();
            var parameterIndex = 0;

            // Filtros por campo
            if (searchDto.Filters?.Any() == true)
            {
                foreach (var filter in searchDto.Filters)
                {
                    var field = fields.FirstOrDefault(f => f.ColumnName.Equals(filter.Key, StringComparison.OrdinalIgnoreCase));
                    if (field != null)
                    {
                        var paramName = $"@param{parameterIndex++}";
                        whereConditions.Add($"[{field.ColumnName}] = {paramName}");
                        parameters.Add(new SqlParameter(paramName, filter.Value ?? DBNull.Value));
                    }
                }
            }

            // Búsqueda de texto
            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                var searchConditions = new List<string>();
                var textFields = fields.Where(f => f.DataType.ToLower().Contains("varchar") || f.DataType.ToLower() == "text");

                foreach (var field in textFields)
                {
                    var paramName = $"@search{parameterIndex++}";
                    searchConditions.Add($"[{field.ColumnName}] LIKE {paramName}");
                    parameters.Add(new SqlParameter(paramName, $"%{searchDto.SearchTerm}%"));
                }

                if (searchConditions.Any())
                {
                    whereConditions.Add($"({string.Join(" OR ", searchConditions)})");
                }
            }

            if (whereConditions.Any())
            {
                queryBuilder.AppendLine($"WHERE {string.Join(" AND ", whereConditions)}");
            }

            // ORDER BY
            var orderByColumn = "[CreatedAt]";
            var orderDirection = "DESC";

            if (!string.IsNullOrEmpty(searchDto.SortField))
            {
                var sortField = fields.FirstOrDefault(f =>
                    f.ColumnName.Equals(searchDto.SortField, StringComparison.OrdinalIgnoreCase));
                if (sortField != null)
                {
                    orderByColumn = $"[{sortField.ColumnName}]";
                    orderDirection = searchDto.SortAscending ? "ASC" : "DESC";
                }
            }

            queryBuilder.AppendLine($"ORDER BY {orderByColumn} {orderDirection}");

            // PAGINATION
            var offset = (searchDto.PageNumber - 1) * searchDto.PageSize;
            queryBuilder.AppendLine($"OFFSET {offset} ROWS FETCH NEXT {searchDto.PageSize} ROWS ONLY");

            // Ejecutar query de datos
            var dataQuery = queryBuilder.ToString();
            var dataResult = await ExecuteDynamicQueryAsync(dataQuery, parameters.ToArray());

            // Query para contar total
            var countQuery = queryBuilder.ToString()
                .Replace($"SELECT {string.Join(", ", selectColumns)}", "SELECT COUNT(*)")
                .Split("ORDER BY")[0]; // Remover ORDER BY y OFFSET para el count

            var totalRecords = await _context.Database.SqlQueryRaw<int>(countQuery, parameters.ToArray()).FirstAsync();

            // Mapear resultados
            var entities = new List<DynamicEntityDTO>();
            foreach (var row in dataResult)
            {
                var entity = _mappingService.MapToDynamicEntity(row, fields);
                entities.Add(entity);
            }

            var totalPages = (int)Math.Ceiling((double)totalRecords / searchDto.PageSize);

            var response = new DynamicEntityListResponseDTO
            {
                Data = entities,
                TotalRecords = totalRecords,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize,
                TotalPages = totalPages,
                HasNextPage = searchDto.PageNumber < totalPages,
                HasPreviousPage = searchDto.PageNumber > 1
            };

            _logger.LogInformation("Retrieved {Count} entities from module {ModuleId}", entities.Count, moduleId);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entities for module {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Gets a single entity by ID
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="entityId">Entity ID</param>
    /// <returns>Entity details</returns>
    public async Task<DynamicEntityDTO> GetEntityByIdAsync(int moduleId, int entityId)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);

            var primaryKeyField = fields.FirstOrDefault(f => f.IsPrimaryKey);
            if (primaryKeyField == null)
            {
                throw new BadRequestException($"Module {module.Name} does not have a primary key field");
            }

            var selectColumns = fields.Select(f => $"[{f.ColumnName}]").ToList();
            selectColumns.AddRange(new[] { "[CreatedAt]", "[ModifiedAt]", "[CreatedByUserId]", "[ModifiedByUserId]" });

            var query = $@"
                SELECT {string.Join(", ", selectColumns)}
                FROM [{module.TableName}]
                WHERE [{primaryKeyField.ColumnName}] = @entityId";

            var parameter = new SqlParameter("@entityId", entityId);
            var result = await ExecuteDynamicQueryAsync(query, parameter);

            if (!result.Any())
            {
                throw new NotFoundException($"Entity with ID {entityId} not found in module {module.Name}");
            }

            var entity = _mappingService.MapToDynamicEntity(result.First(), fields);

            _logger.LogInformation("Retrieved entity {EntityId} from module {ModuleId}", entityId, moduleId);

            return entity;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving entity {EntityId} from module {ModuleId}", entityId, moduleId);
            throw;
        }
    }

    /// <summary>
    /// Creates a new entity
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="userId">User ID creating the entity</param>
    /// <param name="entityDto">Entity data</param>
    /// <returns>Created entity</returns>
    public async Task<DynamicEntityDTO> CreateEntityAsync(int moduleId, int userId, AddUpdateDynamicEntityDTO entityDto)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);

            // Validar datos
            await ValidateEntityDataAsync(moduleId, entityDto.Data);

            // Mapear datos
            var mappedData = _mappingService.MapFromDynamicEntity(entityDto, fields);

            // Agregar campos de auditoría
            mappedData["CreatedAt"] = DateTime.UtcNow;
            mappedData["CreatedByUserId"] = userId;

            // Construir INSERT query
            var insertColumns = new List<string>();
            var insertValues = new List<string>();
            var parameters = new List<SqlParameter>();

            var paramIndex = 0;
            foreach (var data in mappedData)
            {
                insertColumns.Add($"[{data.Key}]");
                var paramName = $"@param{paramIndex++}";
                insertValues.Add(paramName);
                parameters.Add(new SqlParameter(paramName, data.Value ?? DBNull.Value));
            }

            var identityField = fields.FirstOrDefault(f => f.IsIdentity);
            var insertQuery = $@"
                INSERT INTO [{module.TableName}] ({string.Join(", ", insertColumns)})
                VALUES ({string.Join(", ", insertValues)})";

            if (identityField != null)
            {
                insertQuery += $"; SELECT SCOPE_IDENTITY() as [{identityField.ColumnName}]";
                var newId = await _context.Database.SqlQueryRaw<decimal>(insertQuery, parameters.ToArray()).FirstAsync();

                // Obtener el registro completo
                return await GetEntityByIdAsync(moduleId, Convert.ToInt32(newId));
            }
            else
            {
                await _context.Database.ExecuteSqlRawAsync(insertQuery, parameters.ToArray());

                // Para tablas sin identity, necesitamos la PK del DTO
                var primaryKeyField = fields.FirstOrDefault(f => f.IsPrimaryKey);
                if (primaryKeyField != null && entityDto.Data.ContainsKey(primaryKeyField.ColumnName))
                {
                    var pkValue = Convert.ToInt32(entityDto.Data[primaryKeyField.ColumnName]);
                    return await GetEntityByIdAsync(moduleId, pkValue);
                }
                else
                {
                    throw new BadRequestException("Cannot retrieve created entity - no identity or primary key value provided");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity in module {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="userId">User ID updating the entity</param>
    /// <param name="entityDto">Updated entity data</param>
    /// <returns>Updated entity</returns>
    public async Task<DynamicEntityDTO> UpdateEntityAsync(int moduleId, int entityId, int userId, AddUpdateDynamicEntityDTO entityDto)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);

            // Verificar que la entidad existe
            await GetEntityByIdAsync(moduleId, entityId);

            // Validar datos
            await ValidateEntityDataAsync(moduleId, entityDto.Data, entityId);

            // Mapear datos
            var mappedData = _mappingService.MapFromDynamicEntity(entityDto, fields);

            // Agregar campos de auditoría
            mappedData["ModifiedAt"] = DateTime.UtcNow;
            mappedData["ModifiedByUserId"] = userId;

            var primaryKeyField = fields.FirstOrDefault(f => f.IsPrimaryKey);
            if (primaryKeyField == null)
            {
                throw new BadRequestException($"Module {module.Name} does not have a primary key field");
            }

            // Construir UPDATE query
            var setClause = new List<string>();
            var parameters = new List<SqlParameter>();

            var paramIndex = 0;
            foreach (var data in mappedData)
            {
                // No actualizar campos PK, Identity ni CreatedAt/CreatedByUserId
                var field = fields.FirstOrDefault(f => f.ColumnName.Equals(data.Key, StringComparison.OrdinalIgnoreCase));
                if (field != null && (field.IsPrimaryKey || field.IsIdentity))
                    continue;

                if (data.Key.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) ||
                    data.Key.Equals("CreatedByUserId", StringComparison.OrdinalIgnoreCase))
                    continue;

                var paramName = $"@param{paramIndex++}";
                setClause.Add($"[{data.Key}] = {paramName}");
                parameters.Add(new SqlParameter(paramName, data.Value ?? DBNull.Value));
            }

            if (!setClause.Any())
            {
                throw new BadRequestException("No updateable fields provided");
            }

            // Agregar parámetro para WHERE
            var entityIdParam = $"@entityId";
            parameters.Add(new SqlParameter(entityIdParam, entityId));

            var updateQuery = $@"
                UPDATE [{module.TableName}] 
                SET {string.Join(", ", setClause)}
                WHERE [{primaryKeyField.ColumnName}] = {entityIdParam}";

            await _context.Database.ExecuteSqlRawAsync(updateQuery, parameters.ToArray());

            _logger.LogInformation("Updated entity {EntityId} in module {ModuleId}", entityId, moduleId);

            // Retornar entidad actualizada
            return await GetEntityByIdAsync(moduleId, entityId);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity {EntityId} in module {ModuleId}", entityId, moduleId);
            throw;
        }
    }

    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="userId">User ID deleting the entity</param>
    public async Task DeleteEntityAsync(int moduleId, int entityId, int userId)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);

            // Verificar que la entidad existe
            await GetEntityByIdAsync(moduleId, entityId);

            var primaryKeyField = fields.FirstOrDefault(f => f.IsPrimaryKey);
            if (primaryKeyField == null)
            {
                throw new BadRequestException($"Module {module.Name} does not have a primary key field");
            }

            var deleteQuery = $@"
                DELETE FROM [{module.TableName}] 
                WHERE [{primaryKeyField.ColumnName}] = @entityId";

            var parameter = new SqlParameter("@entityId", entityId);
            await _context.Database.ExecuteSqlRawAsync(deleteQuery, parameter);

            _logger.LogInformation("Deleted entity {EntityId} from module {ModuleId} by user {UserId}", entityId, moduleId, userId);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entity {EntityId} from module {ModuleId}", entityId, moduleId);
            throw;
        }
    }

    /// <summary>
    /// Validates entity data against field definitions
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="data">Data to validate</param>
    /// <param name="excludeEntityId">Entity ID to exclude from unique checks</param>
    public async Task ValidateEntityDataAsync(int moduleId, Dictionary<string, object> data, int? excludeEntityId = null)
    {
        try
        {
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);
            var errors = new List<string>();

            foreach (var field in fields)
            {
                var fieldValue = data.ContainsKey(field.ColumnName) ? data[field.ColumnName] : null;

                // Validar campos requeridos
                if (field.IsRequired && !field.IsIdentity && (fieldValue == null || string.IsNullOrWhiteSpace(fieldValue?.ToString())))
                {
                    errors.Add($"Field '{field.DisplayName ?? field.Name}' is required");
                    continue;
                }

                if (fieldValue != null)
                {
                    // Validar tipo de dato
                    if (!_mappingService.IsValidValue(fieldValue, field))
                    {
                        errors.Add($"Invalid value for field '{field.DisplayName ?? field.Name}'");
                        continue;
                    }

                    // Validar reglas personalizadas
                    if (!string.IsNullOrEmpty(field.ValidationRules))
                    {
                        try
                        {
                            var validationRules = JsonSerializer.Deserialize<FieldValidation>(field.ValidationRules);
                            await ValidateFieldRulesAsync(field, fieldValue, validationRules, errors);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Error parsing validation rules for field {FieldName}", field.Name);
                        }
                    }
                }

                // Validar unicidad
                if (field.IsUnique && fieldValue != null)
                {
                    await ValidateUniqueFieldAsync(moduleId, field, fieldValue, excludeEntityId, errors);
                }
            }

            if (errors.Any())
            {
                throw new BadRequestException($"Validation failed: {string.Join("; ", errors)}");
            }
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating entity data for module {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Gets default values for a module
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <returns>Default values</returns>
    public async Task<Dictionary<string, object>> GetDefaultValuesAsync(int moduleId)
    {
        try
        {
            var fields = await _moduleService.GetModuleFieldsAsync(moduleId);
            var defaults = new Dictionary<string, object>();

            foreach (var field in fields.Where(f => !string.IsNullOrEmpty(f.DefaultValue) && !f.IsIdentity))
            {
                try
                {
                    var convertedValue = _mappingService.ConvertValue(field.DefaultValue, field);
                    defaults[field.ColumnName] = convertedValue;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error converting default value for field {FieldName}", field.Name);
                }
            }

            return defaults;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting default values for module {ModuleId}", moduleId);
            throw;
        }
    }

    /// <summary>
    /// Gets lookup data for a field
    /// </summary>
    /// <param name="tableName">Lookup table name</param>
    /// <param name="keyColumn">Key column</param>
    /// <param name="displayColumn">Display column</param>
    /// <param name="searchTerm">Optional search term</param>
    /// <returns>Lookup data</returns>
    public async Task<List<dynamic>> GetLookupDataAsync(string tableName, string keyColumn, string displayColumn, string searchTerm = null)
    {
        try
        {
            var query = new StringBuilder();
            var parameters = new List<SqlParameter>();

            query.AppendLine($"SELECT [{keyColumn}], [{displayColumn}]");
            query.AppendLine($"FROM [{tableName}]");

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query.AppendLine($"WHERE [{displayColumn}] LIKE @searchTerm");
                parameters.Add(new SqlParameter("@searchTerm", $"%{searchTerm}%"));
            }

            query.AppendLine($"ORDER BY [{displayColumn}]");
            query.AppendLine("OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY"); // Limitar resultados

            var result = await ExecuteDynamicQueryAsync(query.ToString(), parameters.ToArray());

            return result.Select(row => (dynamic)new
            {
                Key = row.ContainsKey(keyColumn) ? row[keyColumn] : null,
                Display = row.ContainsKey(displayColumn) ? row[displayColumn] : null
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting lookup data from {TableName}", tableName);
            throw;
        }
    }

    /// <summary>
    /// Executes a dynamic SQL query and returns results as dictionary
    /// </summary>
    /// <param name="query">SQL query</param>
    /// <param name="parameters">SQL parameters</param>
    /// <returns>Query results</returns>
    private async Task<List<Dictionary<string, object>>> ExecuteDynamicQueryAsync(string query, params SqlParameter[] parameters)
    {
        var results = new List<Dictionary<string, object>>();

        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        await connection.OpenAsync();

        using var command = new SqlCommand(query, connection);
        if (parameters?.Any() == true)
        {
            command.Parameters.AddRange(parameters);
        }

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                row[columnName] = value;
            }
            results.Add(row);
        }

        return results;
    }

    /// <summary>
    /// Validates field-specific rules
    /// </summary>
    /// <param name="field">Field definition</param>
    /// <param name="value">Value to validate</param>
    /// <param name="rules">Validation rules</param>
    /// <param name="errors">Error collection</param>
    private async Task ValidateFieldRulesAsync(DynamicFieldDTO field, object value, FieldValidation rules, List<string> errors)
    {
        var stringValue = value?.ToString();

        // Min/Max length para strings
        if (rules.MinLength.HasValue && stringValue != null && stringValue.Length < rules.MinLength.Value)
        {
            errors.Add($"Field '{field.DisplayName ?? field.Name}' must be at least {rules.MinLength.Value} characters");
        }

        if (rules.MaxLength.HasValue && stringValue != null && stringValue.Length > rules.MaxLength.Value)
        {
            errors.Add($"Field '{field.DisplayName ?? field.Name}' must not exceed {rules.MaxLength.Value} characters");
        }

        // Min/Max value para números
        if (rules.MinValue.HasValue && decimal.TryParse(stringValue, out var numValue) && numValue < rules.MinValue.Value)
        {
            errors.Add($"Field '{field.DisplayName ?? field.Name}' must be at least {rules.MinValue.Value}");
        }

        if (rules.MaxValue.HasValue && decimal.TryParse(stringValue, out numValue) && numValue > rules.MaxValue.Value)
        {
            errors.Add($"Field '{field.DisplayName ?? field.Name}' must not exceed {rules.MaxValue.Value}");
        }

        // Valores permitidos
        if (rules.AllowedValues?.Any() == true && !rules.AllowedValues.Contains(stringValue))
        {
            errors.Add($"Field '{field.DisplayName ?? field.Name}' must be one of: {string.Join(", ", rules.AllowedValues)}");
        }

        await Task.CompletedTask; // Para cumplir con la signatura async
    }

    /// <summary>
    /// Validates field uniqueness
    /// </summary>
    /// <param name="moduleId">Module ID</param>
    /// <param name="field">Field definition</param>
    /// <param name="value">Value to check</param>
    /// <param name="excludeEntityId">Entity ID to exclude</param>
    /// <param name="errors">Error collection</param>
    private async Task ValidateUniqueFieldAsync(int moduleId, DynamicFieldDTO field, object value, int? excludeEntityId, List<string> errors)
    {
        try
        {
            var module = await _moduleService.GetModuleByIdAsync(moduleId);
            var primaryKeyField = await _moduleService.GetModuleFieldsAsync(moduleId)
                .ContinueWith(t => t.Result.FirstOrDefault(f => f.IsPrimaryKey));

            var query = new StringBuilder();
            query.AppendLine($"SELECT COUNT(*)");
            query.AppendLine($"FROM [{module.TableName}]");
            query.AppendLine($"WHERE [{field.ColumnName}] = @value");

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@value", value)
            };

            if (excludeEntityId.HasValue && primaryKeyField != null)
            {
                query.AppendLine($"AND [{primaryKeyField.ColumnName}] != @excludeId");
                parameters.Add(new SqlParameter("@excludeId", excludeEntityId.Value));
            }

            var count = await _context.Database.SqlQueryRaw<int>(query.ToString(), parameters.ToArray()).FirstAsync();

            if (count > 0)
            {
                errors.Add($"Field '{field.DisplayName ?? field.Name}' must be unique. Value '{value}' already exists");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error validating uniqueness for field {FieldName}", field.Name);
        }
    }
}
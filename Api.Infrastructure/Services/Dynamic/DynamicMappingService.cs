//using Api.Infrastructure.Services.Dynamic;
//using Api.Shared.DTOs.Dynamic;
//using Microsoft.Extensions.Logging;
//using System.Globalization;

//namespace Api.Infrastructure.Services.Dynamic;

//public class DynamicMappingService : IDynamicMappingService
//{
//    private readonly ILogger<DynamicMappingService> _logger;

//    public DynamicMappingService(ILogger<DynamicMappingService> logger)
//    {
//        _logger = logger;
//    }

//    /// <summary>
//    /// Maps database row to dynamic entity DTO
//    /// </summary>
//    /// <param name="dbData">Database row data</param>
//    /// <param name="fields">Field definitions</param>
//    /// <returns>Dynamic entity DTO</returns>
//    public DynamicEntityDTO MapToDynamicEntity(Dictionary<string, object> dbData, List<DynamicFieldDTO> fields)
//    {
//        try
//        {
//            var entity = new DynamicEntityDTO();

//            // Mapear campos de auditoría estándar
//            entity.CreatedAt = GetValueOrDefault<DateTime>(dbData, "CreatedAt");
//            entity.CreatedByUserId = GetValueOrDefault<int>(dbData, "CreatedByUserId");
//            entity.ModifiedAt = GetValueOrDefault<DateTime?>(dbData, "ModifiedAt");
//            entity.ModifiedByUserId = GetValueOrDefault<int?>(dbData, "ModifiedByUserId");

//            // Obtener ID de la entidad (del primer campo PK)
//            var primaryKeyField = fields.FirstOrDefault(f => f.IsPrimaryKey);
//            if (primaryKeyField != null && dbData.ContainsKey(primaryKeyField.ColumnName))
//            {
//                entity.Id = Convert.ToInt32(dbData[primaryKeyField.ColumnName]);
//            }

//            // Mapear campos dinámicos
//            foreach (var field in fields)
//            {
//                if (dbData.ContainsKey(field.ColumnName))
//                {
//                    var value = dbData[field.ColumnName];
//                    entity.Data[field.ColumnName] = ConvertFromDbValue(value, field);
//                }
//            }

//            return entity;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error mapping database data to dynamic entity");
//            throw;
//        }
//    }

//    /// <summary>
//    /// Maps dynamic entity DTO to database values
//    /// </summary>
//    /// <param name="entityDto">Entity DTO</param>
//    /// <param name="fields">Field definitions</param>
//    /// <returns>Database values</returns>
//    public Dictionary<string, object> MapFromDynamicEntity(AddUpdateDynamicEntityDTO entityDto, List<DynamicFieldDTO> fields)
//    {
//        try
//        {
//            var dbData = new Dictionary<string, object>();

//            foreach (var field in fields)
//            {
//                // Saltar campos identity
//                if (field.IsIdentity)
//                    continue;

//                if (entityDto.Data.ContainsKey(field.ColumnName))
//                {
//                    var value = entityDto.Data[field.ColumnName];
//                    dbData[field.ColumnName] = ConvertToDbValue(value, field);
//                }
//                else if (field.IsRequired && !string.IsNullOrEmpty(field.DefaultValue))
//                {
//                    // Usar valor por defecto si el campo es requerido
//                    dbData[field.ColumnName] = ConvertValue(field.DefaultValue, field);
//                }
//            }

//            return dbData;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error mapping dynamic entity to database values");
//            throw;
//        }
//    }

//    /// <summary>
//    /// Converts a value to the appropriate type for a field
//    /// </summary>
//    /// <param name="value">Value to convert</param>
//    /// <param name="field">Field definition</param>
//    /// <returns>Converted value</returns>
//    public object ConvertValue(object value, DynamicFieldDTO field)
//    {
//        try
//        {
//            if (value == null || value == DBNull.Value)
//                return null;

//            var stringValue = value.ToString();
//            if (string.IsNullOrEmpty(stringValue))
//                return null;

//            return field.DataType.ToLower() switch
//            {
//                "varchar" or "nvarchar" or "text" or "ntext" => stringValue,

//                "int" => Convert.ToInt32(value),
//                "bigint" => Convert.ToInt64(value),

//                "decimal" => Convert.ToDecimal(value, CultureInfo.InvariantCulture),
//                "float" => Convert.ToDouble(value, CultureInfo.InvariantCulture),

//                "datetime" => ConvertToDateTime(value),
//                "date" => ConvertToDateTime(value)?.Date,
//                "time" => ConvertToTimeSpan(value),

//                "bit" => ConvertToBoolean(value),

//                "uniqueidentifier" => ConvertToGuid(value),

//                _ => stringValue
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.LogWarning(ex, "Error converting value '{Value}' for field '{FieldName}' of type '{DataType}'",
//                value, field.Name, field.DataType);
//            throw new FormatException($"Cannot convert value '{value}' to {field.DataType} for field '{field.Name}'");
//        }
//    }

//    /// <summary>
//    /// Validates if a value is valid for a field
//    /// </summary>
//    /// <param name="value">Value to validate</param>
//    /// <param name="field">Field definition</param>
//    /// <returns>True if valid</returns>
//    public bool IsValidValue(object value, DynamicFieldDTO field)
//    {
//        try
//        {
//            if (value == null)
//                return !field.IsRequired;

//            ConvertValue(value, field);
//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    /// Gets CLR type for SQL data type
//    /// </summary>
//    /// <param name="sqlDataType">SQL data type</param>
//    /// <returns>CLR type</returns>
//    public Type GetClrType(string sqlDataType)
//    {
//        return sqlDataType.ToLower() switch
//        {
//            "varchar" or "nvarchar" or "text" or "ntext" => typeof(string),
//            "int" => typeof(int),
//            "bigint" => typeof(long),
//            "decimal" => typeof(decimal),
//            "float" => typeof(double),
//            "datetime" or "date" => typeof(DateTime),
//            "time" => typeof(TimeSpan),
//            "bit" => typeof(bool),
//            "uniqueidentifier" => typeof(Guid),
//            _ => typeof(string)
//        };
//    }

//    /// <summary>
//    /// Gets SQL data type for CLR type
//    /// </summary>
//    /// <param name="clrType">CLR type</param>
//    /// <param name="maxLength">Maximum length</param>
//    /// <param name="precision">Precision</param>
//    /// <param name="scale">Scale</param>
//    /// <returns>SQL data type</returns>
//    public string GetSqlDataType(Type clrType, int? maxLength = null, int? precision = null, int? scale = null)
//    {
//        if (clrType == typeof(string))
//        {
//            return $"NVARCHAR({maxLength ?? 255})";
//        }
//        else if (clrType == typeof(int) || clrType == typeof(int?))
//        {
//            return "INT";
//        }
//        else if (clrType == typeof(long) || clrType == typeof(long?))
//        {
//            return "BIGINT";
//        }
//        else if (clrType == typeof(decimal) || clrType == typeof(decimal?))
//        {
//            return $"DECIMAL({precision ?? 18},{scale ?? 2})";
//        }
//        else if (clrType == typeof(double) || clrType == typeof(double?) ||
//                 clrType == typeof(float) || clrType == typeof(float?))
//        {
//            return "FLOAT";
//        }
//        else if (clrType == typeof(DateTime) || clrType == typeof(DateTime?))
//        {
//            return "DATETIME2";
//        }
//        else if (clrType == typeof(TimeSpan) || clrType == typeof(TimeSpan?))
//        {
//            return "TIME";
//        }
//        else if (clrType == typeof(bool) || clrType == typeof(bool?))
//        {
//            return "BIT";
//        }
//        else if (clrType == typeof(Guid) || clrType == typeof(Guid?))
//        {
//            return "UNIQUEIDENTIFIER";
//        }
//        else
//        {
//            return $"NVARCHAR({maxLength ?? 255})";
//        }
//    }

//    /// <summary>
//    /// Converts database value to display value
//    /// </summary>
//    /// <param name="value">Database value</param>
//    /// <param name="field">Field definition</param>
//    /// <returns>Display value</returns>
//    private object ConvertFromDbValue(object value, DynamicFieldDTO field)
//    {
//        if (value == null || value == DBNull.Value)
//            return null;

//        // Para la mayoría de los casos, retornamos el valor tal como viene de la DB
//        // Solo hacemos conversiones especiales cuando es necesario
//        return field.DataType.ToLower() switch
//        {
//            "bit" => Convert.ToBoolean(value),
//            "date" => ((DateTime)value).Date,
//            _ => value
//        };
//    }

//    /// <summary>
//    /// Converts entity value to database value
//    /// </summary>
//    /// <param name="value">Entity value</param>
//    /// <param name="field">Field definition</param>
//    /// <returns>Database value</returns>
//    private object ConvertToDbValue(object value, DynamicFieldDTO field)
//    {
//        if (value == null)
//            return DBNull.Value;

//        // Convertir usando la lógica principal
//        var convertedValue = ConvertValue(value, field);
//        return convertedValue ?? DBNull.Value;
//    }

//    /// <summary>
//    /// Gets value from dictionary with default
//    /// </summary>
//    /// <typeparam name="T">Target type</typeparam>
//    /// <param name="data">Data dictionary</param>
//    /// <param name="key">Key</param>
//    /// <returns>Value or default</returns>
//    private T GetValueOrDefault<T>(Dictionary<string, object> data, string key)
//    {
//        if (data.ContainsKey(key) && data[key] != null && data[key] != DBNull.Value)
//        {
//            try
//            {
//                if (data[key] is T directValue)
//                    return directValue;

//                return (T)Convert.ChangeType(data[key], typeof(T));
//            }
//            catch
//            {
//                // Si falla la conversión, retornar default
//            }
//        }

//        return default(T);
//    }

//    /// <summary>
//    /// Converts value to DateTime
//    /// </summary>
//    /// <param name="value">Value to convert</param>
//    /// <returns>DateTime value</returns>
//    private DateTime? ConvertToDateTime(object value)
//    {
//        if (value == null)
//            return null;

//        if (value is DateTime dateTime)
//            return dateTime;

//        if (DateTime.TryParse(value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
//            return parsed;

//        throw new FormatException($"Cannot convert '{value}' to DateTime");
//    }

//    /// <summary>
//    /// Converts value to TimeSpan
//    /// </summary>
//    /// <param name="value">Value to convert</param>
//    /// <returns>TimeSpan value</returns>
//    private TimeSpan? ConvertToTimeSpan(object value)
//    {
//        if (value == null)
//            return null;

//        if (value is TimeSpan timeSpan)
//            return timeSpan;

//        if (value is DateTime dateTime)
//            return dateTime.TimeOfDay;

//        if (TimeSpan.TryParse(value.ToString(), CultureInfo.InvariantCulture, out var parsed))
//            return parsed;

//        throw new FormatException($"Cannot convert '{value}' to TimeSpan");
//    }

//    /// <summary>
//    /// Converts value to Boolean
//    /// </summary>
//    /// <param name="value">Value to convert</param>
//    /// <returns>Boolean value</returns>
//    private bool ConvertToBoolean(object value)
//    {
//        if (value == null)
//            return false;

//        if (value is bool boolValue)
//            return boolValue;

//        var stringValue = value.ToString().ToLower();

//        return stringValue switch
//        {
//            "true" or "1" or "yes" or "on" or "sí" => true,
//            "false" or "0" or "no" or "off" => false,
//            _ => throw new FormatException($"Cannot convert '{value}' to Boolean")
//        };
//    }

//    /// <summary>
//    /// Converts value to Guid
//    /// </summary>
//    /// <param name="value">Value to convert</param>
//    /// <returns>Guid value</returns>
//    private Guid? ConvertToGuid(object value)
//    {
//        if (value == null)
//            return null;

//        if (value is Guid guid)
//            return guid;

//        if (Guid.TryParse(value.ToString(), out var parsed))
//            return parsed;

//        throw new FormatException($"Cannot convert '{value}' to Guid");
//    }
//}

///// <summary>
///// Clase auxiliar para validaciones de campos
///// </summary>
//public class FieldValidation
//{
//    public int? MinLength { get; set; }
//    public int? MaxLength { get; set; }
//    public decimal? MinValue { get; set; }
//    public decimal? MaxValue { get; set; }
//    public string Pattern { get; set; } // Regex pattern
//    public string CustomValidation { get; set; } // Nombre de validación personalizada
//    public List<string> AllowedValues { get; set; } // Para enums/selects
//}
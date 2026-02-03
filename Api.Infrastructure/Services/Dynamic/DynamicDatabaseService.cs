//using Api.Infrastructure.Exceptions;
//using Api.Infrastructure.Services.Dynamic;
//using Api.Shared.Data;
//using Api.Shared.DTOs.Dynamic;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System.Data;
//using System.Text;

//namespace Api.Infrastructure.Services.Dynamic;

//public class DynamicDatabaseService : IDynamicDatabaseService
//{
//    private readonly Context _context;
//    private readonly ILogger<DynamicDatabaseService> _logger;

//    public DynamicDatabaseService(
//        Context context,
//        ILogger<DynamicDatabaseService> logger)
//    {
//        _context = context;
//        _logger = logger;
//    }

//    /// <summary>
//    /// Creates a physical table based on module definition
//    /// </summary>
//    /// <param name="moduleDto">Module definition</param>
//    public async Task CreateTableAsync(DynamicModuleDTO moduleDto)
//    {
//        try
//        {
//            var script = await GenerateCreateTableScriptAsync(moduleDto);

//            _logger.LogInformation("Creating table {TableName} with script: {Script}", moduleDto.TableName, script);

//            await _context.Database.ExecuteSqlRawAsync(script);

//            _logger.LogInformation("Successfully created table {TableName}", moduleDto.TableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error creating table {TableName}", moduleDto.TableName);
//            throw new BadRequestException($"Error creating table '{moduleDto.TableName}': {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// Drops a physical table
//    /// </summary>
//    /// <param name="tableName">Table name to drop</param>
//    public async Task DropTableAsync(string tableName)
//    {
//        try
//        {
//            if (!await TableExistsAsync(tableName))
//            {
//                _logger.LogWarning("Table {TableName} does not exist, skipping drop", tableName);
//                return;
//            }

//            var script = $"DROP TABLE [{tableName}]";

//            _logger.LogInformation("Dropping table {TableName}", tableName);

//            await _context.Database.ExecuteSqlRawAsync(script);

//            _logger.LogInformation("Successfully dropped table {TableName}", tableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error dropping table {TableName}", tableName);
//            throw new BadRequestException($"Error dropping table '{tableName}': {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// Adds a column to an existing table
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="fieldDto">Field definition</param>
//    public async Task AlterTableAddColumnAsync(string tableName, DynamicFieldDTO fieldDto)
//    {
//        try
//        {
//            var script = await GenerateAlterTableScriptAsync(tableName, fieldDto, "ADD");

//            _logger.LogInformation("Adding column {ColumnName} to table {TableName}", fieldDto.ColumnName, tableName);

//            await _context.Database.ExecuteSqlRawAsync(script);

//            _logger.LogInformation("Successfully added column {ColumnName} to table {TableName}", fieldDto.ColumnName, tableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error adding column {ColumnName} to table {TableName}", fieldDto.ColumnName, tableName);
//            throw new BadRequestException($"Error adding column '{fieldDto.ColumnName}' to table '{tableName}': {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// Drops a column from an existing table
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="columnName">Column name to drop</param>
//    public async Task AlterTableDropColumnAsync(string tableName, string columnName)
//    {
//        try
//        {
//            if (!await ColumnExistsAsync(tableName, columnName))
//            {
//                _logger.LogWarning("Column {ColumnName} does not exist in table {TableName}, skipping drop", columnName, tableName);
//                return;
//            }

//            var script = $"ALTER TABLE [{tableName}] DROP COLUMN [{columnName}]";

//            _logger.LogInformation("Dropping column {ColumnName} from table {TableName}", columnName, tableName);

//            await _context.Database.ExecuteSqlRawAsync(script);

//            _logger.LogInformation("Successfully dropped column {ColumnName} from table {TableName}", columnName, tableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error dropping column {ColumnName} from table {TableName}", columnName, tableName);
//            throw new BadRequestException($"Error dropping column '{columnName}' from table '{tableName}': {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// Modifies an existing column (limited support)
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="fieldDto">Field definition</param>
//    public async Task AlterTableModifyColumnAsync(string tableName, DynamicFieldDTO fieldDto)
//    {
//        try
//        {
//            var script = await GenerateAlterTableScriptAsync(tableName, fieldDto, "ALTER");

//            _logger.LogInformation("Modifying column {ColumnName} in table {TableName}", fieldDto.ColumnName, tableName);

//            await _context.Database.ExecuteSqlRawAsync(script);

//            _logger.LogInformation("Successfully modified column {ColumnName} in table {TableName}", fieldDto.ColumnName, tableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error modifying column {ColumnName} in table {TableName}", fieldDto.ColumnName, tableName);
//            throw new BadRequestException($"Error modifying column '{fieldDto.ColumnName}' in table '{tableName}': {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// Checks if a table exists
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <returns>True if table exists</returns>
//    public async Task<bool> TableExistsAsync(string tableName)
//    {
//        try
//        {
//            var query = @"
//                SELECT COUNT(*) 
//                FROM INFORMATION_SCHEMA.TABLES 
//                WHERE TABLE_NAME = @tableName 
//                AND TABLE_TYPE = 'BASE TABLE'";

//            var parameter = new SqlParameter("@tableName", tableName);
//            var result = await _context.Database.SqlQueryRaw<int>(query, parameter).FirstAsync();

//            return result > 0;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error checking if table {TableName} exists", tableName);
//            return false;
//        }
//    }

//    /// <summary>
//    /// Checks if a column exists in a table
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="columnName">Column name</param>
//    /// <returns>True if column exists</returns>
//    public async Task<bool> ColumnExistsAsync(string tableName, string columnName)
//    {
//        try
//        {
//            var query = @"
//                SELECT COUNT(*) 
//                FROM INFORMATION_SCHEMA.COLUMNS 
//                WHERE TABLE_NAME = @tableName 
//                AND COLUMN_NAME = @columnName";

//            var parameters = new[]
//            {
//                new SqlParameter("@tableName", tableName),
//                new SqlParameter("@columnName", columnName)
//            };

//            var result = await _context.Database.SqlQueryRaw<int>(query, parameters).FirstAsync();

//            return result > 0;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error checking if column {ColumnName} exists in table {TableName}", columnName, tableName);
//            return false;
//        }
//    }

//    /// <summary>
//    /// Gets all columns for a table
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <returns>List of column names</returns>
//    public async Task<List<string>> GetTableColumnsAsync(string tableName)
//    {
//        try
//        {
//            var query = @"
//                SELECT COLUMN_NAME 
//                FROM INFORMATION_SCHEMA.COLUMNS 
//                WHERE TABLE_NAME = @tableName 
//                ORDER BY ORDINAL_POSITION";

//            var parameter = new SqlParameter("@tableName", tableName);
//            var columns = await _context.Database.SqlQueryRaw<string>(query, parameter).ToListAsync();

//            return columns;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error getting columns for table {TableName}", tableName);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Generates CREATE TABLE script
//    /// </summary>
//    /// <param name="moduleDto">Module definition</param>
//    /// <returns>SQL script</returns>
//    public async Task<string> GenerateCreateTableScriptAsync(DynamicModuleDTO moduleDto)
//    {
//        try
//        {
//            var script = new StringBuilder();
//            script.AppendLine($"CREATE TABLE [{moduleDto.TableName}] (");

//            var columnDefinitions = new List<string>();

//            // Agregar campos definidos por el usuario
//            foreach (var field in moduleDto.Fields.OrderBy(f => f.SortOrder))
//            {
//                columnDefinitions.Add(GenerateColumnDefinition(field));
//            }

//            // Agregar campos de auditoría estándar
//            columnDefinitions.Add("[CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE()");
//            columnDefinitions.Add("[CreatedByUserId] INT NOT NULL");
//            columnDefinitions.Add("[ModifiedAt] DATETIME2 NULL");
//            columnDefinitions.Add("[ModifiedByUserId] INT NULL");

//            script.AppendLine(string.Join(",\n    ", columnDefinitions));

//            // Agregar constraint de primary key
//            var primaryKeyFields = moduleDto.Fields.Where(f => f.IsPrimaryKey).ToList();
//            if (primaryKeyFields.Any())
//            {
//                var pkColumns = string.Join(", ", primaryKeyFields.Select(f => $"[{f.ColumnName}]"));
//                script.AppendLine($",    CONSTRAINT [PK_{moduleDto.TableName}] PRIMARY KEY ({pkColumns})");
//            }

//            script.AppendLine(")");

//            return script.ToString();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error generating CREATE TABLE script for {TableName}", moduleDto.TableName);
//            throw;
//        }

//        await Task.CompletedTask; // Para cumplir con la signatura async
//    }

//    /// <summary>
//    /// Generates ALTER TABLE script
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="fieldDto">Field definition</param>
//    /// <param name="operation">Operation (ADD, ALTER, DROP)</param>
//    /// <returns>SQL script</returns>
//    public async Task<string> GenerateAlterTableScriptAsync(string tableName, DynamicFieldDTO fieldDto, string operation)
//    {
//        try
//        {
//            return operation.ToUpper() switch
//            {
//                "ADD" => $"ALTER TABLE [{tableName}] ADD {GenerateColumnDefinition(fieldDto)}",
//                "ALTER" => $"ALTER TABLE [{tableName}] ALTER COLUMN {GenerateColumnDefinition(fieldDto, false)}",
//                "DROP" => $"ALTER TABLE [{tableName}] DROP COLUMN [{fieldDto.ColumnName}]",
//                _ => throw new ArgumentException($"Unsupported operation: {operation}")
//            };
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error generating ALTER TABLE script for {TableName}, operation {Operation}", tableName, operation);
//            throw;
//        }

//        await Task.CompletedTask; // Para cumplir con la signatura async
//    }

//    /// <summary>
//    /// Validates table structure against module definition
//    /// </summary>
//    /// <param name="moduleDto">Module definition</param>
//    public async Task ValidateTableStructureAsync(DynamicModuleDTO moduleDto)
//    {
//        try
//        {
//            if (!await TableExistsAsync(moduleDto.TableName))
//            {
//                throw new NotFoundException($"Table '{moduleDto.TableName}' does not exist");
//            }

//            var existingColumns = await GetTableColumnsAsync(moduleDto.TableName);
//            var definedColumns = moduleDto.Fields.Select(f => f.ColumnName).ToList();

//            // Verificar que todas las columnas definidas existen
//            var missingColumns = definedColumns.Except(existingColumns, StringComparer.OrdinalIgnoreCase).ToList();
//            if (missingColumns.Any())
//            {
//                throw new BadRequestException($"Missing columns in table '{moduleDto.TableName}': {string.Join(", ", missingColumns)}");
//            }

//            _logger.LogInformation("Table structure validation passed for {TableName}", moduleDto.TableName);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error validating table structure for {TableName}", moduleDto.TableName);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Checks if a column can be safely dropped
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <param name="columnName">Column name</param>
//    /// <returns>True if can be dropped</returns>
//    public async Task<bool> CanDropColumnAsync(string tableName, string columnName)
//    {
//        try
//        {
//            // Verificar si la columna es parte de una clave primaria
//            var pkCheckQuery = @"
//                SELECT COUNT(*) 
//                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
//                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc 
//                    ON kcu.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
//                WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 
//                AND kcu.TABLE_NAME = @tableName 
//                AND kcu.COLUMN_NAME = @columnName";

//            var parameters = new[]
//            {
//                new SqlParameter("@tableName", tableName),
//                new SqlParameter("@columnName", columnName)
//            };

//            var isPrimaryKey = await _context.Database.SqlQueryRaw<int>(pkCheckQuery, parameters).FirstAsync();

//            if (isPrimaryKey > 0)
//            {
//                _logger.LogWarning("Cannot drop column {ColumnName} from {TableName} - it's part of primary key", columnName, tableName);
//                return false;
//            }

//            // Verificar si tiene constraints de foreign key
//            var fkCheckQuery = @"
//                SELECT COUNT(*) 
//                FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu
//                INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc 
//                    ON kcu.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
//                WHERE kcu.TABLE_NAME = @tableName 
//                AND kcu.COLUMN_NAME = @columnName";

//            var hasForeignKey = await _context.Database.SqlQueryRaw<int>(fkCheckQuery, parameters).FirstAsync();

//            if (hasForeignKey > 0)
//            {
//                _logger.LogWarning("Cannot drop column {ColumnName} from {TableName} - it has foreign key constraints", columnName, tableName);
//                return false;
//            }

//            return true;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error checking if column {ColumnName} can be dropped from {TableName}", columnName, tableName);
//            return false;
//        }
//    }

//    /// <summary>
//    /// Generates column definition for SQL script
//    /// </summary>
//    /// <param name="field">Field definition</param>
//    /// <param name="includeColumnName">Whether to include column name (for ADD vs ALTER)</param>
//    /// <returns>Column definition string</returns>
//    private string GenerateColumnDefinition(DynamicFieldDTO field, bool includeColumnName = true)
//    {
//        var definition = new StringBuilder();

//        if (includeColumnName)
//        {
//            definition.Append($"[{field.ColumnName}] ");
//        }

//        // Tipo de dato
//        switch (field.DataType.ToLower())
//        {
//            case "varchar":
//                definition.Append($"VARCHAR({field.MaxLength ?? 255})");
//                break;
//            case "nvarchar":
//                definition.Append($"NVARCHAR({field.MaxLength ?? 255})");
//                break;
//            case "int":
//                definition.Append("INT");
//                break;
//            case "bigint":
//                definition.Append("BIGINT");
//                break;
//            case "decimal":
//                definition.Append($"DECIMAL({field.Precision ?? 18},{field.Scale ?? 2})");
//                break;
//            case "float":
//                definition.Append("FLOAT");
//                break;
//            case "datetime":
//                definition.Append("DATETIME2");
//                break;
//            case "date":
//                definition.Append("DATE");
//                break;
//            case "time":
//                definition.Append("TIME");
//                break;
//            case "bit":
//                definition.Append("BIT");
//                break;
//            case "uniqueidentifier":
//                definition.Append("UNIQUEIDENTIFIER");
//                break;
//            case "text":
//                definition.Append("TEXT");
//                break;
//            case "ntext":
//                definition.Append("NTEXT");
//                break;
//            default:
//                throw new ArgumentException($"Unsupported data type: {field.DataType}");
//        }

//        // Identity
//        if (field.IsIdentity)
//        {
//            definition.Append(" IDENTITY(1,1)");
//        }

//        // Nullable
//        if (field.IsRequired)
//        {
//            definition.Append(" NOT NULL");
//        }
//        else
//        {
//            definition.Append(" NULL");
//        }

//        // Default value
//        if (!string.IsNullOrEmpty(field.DefaultValue) && !field.IsIdentity)
//        {
//            if (field.DataType.ToLower() == "bit")
//            {
//                definition.Append($" DEFAULT {field.DefaultValue}");
//            }
//            else if (new[] { "varchar", "nvarchar", "text", "ntext", "date", "datetime", "time" }.Contains(field.DataType.ToLower()))
//            {
//                definition.Append($" DEFAULT '{field.DefaultValue}'");
//            }
//            else
//            {
//                definition.Append($" DEFAULT {field.DefaultValue}");
//            }
//        }

//        // Unique constraint
//        if (field.IsUnique && !field.IsPrimaryKey)
//        {
//            // Se manejará por separado con ALTER TABLE ADD CONSTRAINT
//        }

//        return definition.ToString();
//    }
//}
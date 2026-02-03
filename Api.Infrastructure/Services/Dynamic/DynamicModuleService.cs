//using Api.Infrastructure.Exceptions;
//using Api.Infrastructure.Services.Dynamic;
//using Api.Shared.Data;
//using Api.Shared.DTOs.Dynamic;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System.Text.Json;

//namespace Api.Infrastructure.Services.Dynamic;

//public class DynamicModuleService : IDynamicModuleService
//{
//    private readonly IMapper _mapper;
//    private readonly Context _context;
//    private readonly ILogger<DynamicModuleService> _logger;
//    private readonly IDynamicDatabaseService _databaseService;

//    public DynamicModuleService(
//        IMapper mapper,
//        Context context,
//        ILogger<DynamicModuleService> logger,
//        IDynamicDatabaseService databaseService)
//    {
//        _mapper = mapper;
//        _context = context;
//        _logger = logger;
//        _databaseService = databaseService;
//    }

//    /// <summary>
//    /// Retrieves all dynamic modules
//    /// </summary>
//    /// <returns>List of modules</returns>
//    public async Task<List<DynamicModuleDTO>> GetAllModulesAsync()
//    {
//        try
//        {
//            var modules = await _context.DynamicModules
//                .Include(m => m.DynamicFields.OrderBy(f => f.SortOrder))
//                .AsNoTracking()
//                .Where(m => m.IsActive)
//                .OrderBy(m => m.Name)
//                .ToListAsync();

//            _logger.LogInformation("Retrieved {Count} dynamic modules", modules.Count);

//            return _mapper.Map<List<DynamicModuleDTO>>(modules);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving all dynamic modules");
//            throw;
//        }
//    }

//    /// <summary>
//    /// Retrieves a module by its ID
//    /// </summary>
//    /// <param name="moduleId">Module ID</param>
//    /// <returns>Module details</returns>
//    public async Task<DynamicModuleDTO> GetModuleByIdAsync(int moduleId)
//    {
//        try
//        {
//            var module = await _context.DynamicModules
//                .Include(m => m.DynamicFields.OrderBy(f => f.SortOrder))
//                .AsNoTracking()
//                .FirstOrDefaultAsync(m => m.ModuleId == moduleId && m.IsActive);

//            if (module == null)
//            {
//                _logger.LogWarning("Dynamic module with ID {ModuleId} not found", moduleId);
//                throw new NotFoundException($"Dynamic module with ID {moduleId} not found");
//            }

//            return _mapper.Map<DynamicModuleDTO>(module);
//        }
//        catch (NotFoundException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving dynamic module {ModuleId}", moduleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Gets modules with statistics
//    /// </summary>
//    /// <returns>List of modules with stats</returns>
//    public async Task<List<DynamicModuleWithStatsDTO>> GetModulesWithStatsAsync()
//    {
//        try
//        {
//            var modules = await _context.DynamicModules
//                .AsNoTracking()
//                .Where(m => m.IsActive)
//                .Select(m => new DynamicModuleWithStatsDTO
//                {
//                    ModuleId = m.ModuleId,
//                    Name = m.Name,
//                    Description = m.Description,
//                    TableName = m.TableName,
//                    EntityName = m.EntityName,
//                    IsActive = m.IsActive,
//                    FieldCount = m.DynamicFields.Count(),
//                    RecordCount = 0, // Se calculará dinámicamente
//                    CreatedAt = m.CreatedAt,
//                    ModifiedAt = m.ModifiedAt
//                })
//                .OrderBy(m => m.Name)
//                .ToListAsync();

//            // Calcular la cantidad de registros para cada tabla
//            foreach (var module in modules)
//            {
//                try
//                {
//                    if (await _databaseService.TableExistsAsync(module.TableName))
//                    {
//                        var countQuery = $"SELECT COUNT(*) FROM [{module.TableName}]";
//                        module.RecordCount = await _context.Database.SqlQueryRaw<int>(countQuery).FirstAsync();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogWarning(ex, "Could not get record count for table {TableName}", module.TableName);
//                    module.RecordCount = 0;
//                }
//            }

//            _logger.LogInformation("Retrieved {Count} dynamic modules with statistics", modules.Count);

//            return modules;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving dynamic modules with statistics");
//            throw;
//        }
//    }

//    /// <summary>
//    /// Gets a single module with statistics
//    /// </summary>
//    /// <param name="moduleId">Module ID</param>
//    /// <returns>Module with statistics</returns>
//    public async Task<DynamicModuleWithStatsDTO> GetModuleWithStatsAsync(int moduleId)
//    {
//        try
//        {
//            var module = await _context.DynamicModules
//                .AsNoTracking()
//                .Where(m => m.ModuleId == moduleId && m.IsActive)
//                .Select(m => new DynamicModuleWithStatsDTO
//                {
//                    ModuleId = m.ModuleId,
//                    Name = m.Name,
//                    Description = m.Description,
//                    TableName = m.TableName,
//                    EntityName = m.EntityName,
//                    IsActive = m.IsActive,
//                    FieldCount = m.DynamicFields.Count(),
//                    RecordCount = 0,
//                    CreatedAt = m.CreatedAt,
//                    ModifiedAt = m.ModifiedAt
//                })
//                .FirstOrDefaultAsync();

//            if (module == null)
//            {
//                throw new NotFoundException($"Dynamic module with ID {moduleId} not found");
//            }

//            // Calcular cantidad de registros
//            try
//            {
//                if (await _databaseService.TableExistsAsync(module.TableName))
//                {
//                    var countQuery = $"SELECT COUNT(*) FROM [{module.TableName}]";
//                    module.RecordCount = await _context.Database.SqlQueryRaw<int>(countQuery).FirstAsync();
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogWarning(ex, "Could not get record count for table {TableName}", module.TableName);
//                module.RecordCount = 0;
//            }

//            return module;
//        }
//        catch (NotFoundException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving dynamic module {ModuleId} with statistics", moduleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Creates a new dynamic module and its table
//    /// </summary>
//    /// <param name="userId">User ID creating the module</param>
//    /// <param name="moduleDto">Module data</param>
//    /// <returns>Created module</returns>
//    public async Task<DynamicModuleDTO> CreateModuleAsync(int userId, AddDynamicModuleDTO moduleDto)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            // Validar los datos del módulo
//            await ValidateModuleDefinitionAsync(moduleDto);

//            // Validar que no existe un módulo con el mismo nombre
//            if (await ModuleExistsAsync(moduleDto.Name))
//            {
//                throw new BadRequestException($"A module with the name '{moduleDto.Name}' already exists");
//            }

//            // Validar que no existe una tabla con el mismo nombre
//            if (await _databaseService.TableExistsAsync(moduleDto.TableName))
//            {
//                throw new BadRequestException($"A table with the name '{moduleDto.TableName}' already exists");
//            }

//            // Crear el módulo en la base de datos
//            var module = _mapper.Map<DynamicModule>(moduleDto);
//            module.CreatedAt = DateTime.UtcNow;
//            module.CreatedByUserId = userId;

//            // Asignar orden a los campos
//            for (int i = 0; i < module.DynamicFields.Count; i++)
//            {
//                module.DynamicFields.ElementAt(i).SortOrder = i + 1;
//                module.DynamicFields.ElementAt(i).CreatedAt = DateTime.UtcNow;
//                module.DynamicFields.ElementAt(i).CreatedByUserId = userId;
//            }

//            _context.DynamicModules.Add(module);
//            await _context.SaveChangesAsync();

//            // Crear la tabla física en la base de datos
//            var moduleDto_result = _mapper.Map<DynamicModuleDTO>(module);
//            await _databaseService.CreateTableAsync(moduleDto_result);

//            await transaction.CommitAsync();

//            _logger.LogInformation(
//                "Dynamic module '{ModuleName}' with table '{TableName}' created by user {UserId}",
//                module.Name,
//                module.TableName,
//                userId);

//            return moduleDto_result;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, "Error creating dynamic module for user {UserId}", userId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Updates an existing module (only metadata, not schema)
//    /// </summary>
//    /// <param name="userId">User ID performing the update</param>
//    /// <param name="moduleDto">Updated module data</param>
//    /// <returns>Updated module</returns>
//    public async Task<DynamicModuleDTO> UpdateModuleAsync(int userId, UpdateDynamicModuleDTO moduleDto)
//    {
//        try
//        {
//            await ValidateModuleUpdateAsync(moduleDto);

//            var module = await _context.DynamicModules
//                .Include(m => m.DynamicFields)
//                .FirstOrDefaultAsync(m => m.ModuleId == moduleDto.ModuleId);

//            if (module == null)
//            {
//                throw new NotFoundException($"Dynamic module with ID {moduleDto.ModuleId} not found");
//            }

//            // Validar que no existe otro módulo con el mismo nombre
//            if (await ModuleExistsAsync(moduleDto.Name, moduleDto.ModuleId))
//            {
//                throw new BadRequestException($"A module with the name '{moduleDto.Name}' already exists");
//            }

//            // Actualizar solo propiedades de metadata (no estructura de tabla)
//            module.Name = moduleDto.Name;
//            module.Description = moduleDto.Description;
//            module.IsActive = moduleDto.IsActive;
//            module.ModifiedAt = DateTime.UtcNow;
//            module.ModifiedByUserId = userId;

//            await _context.SaveChangesAsync();

//            _logger.LogInformation(
//                "Dynamic module '{ModuleName}' updated by user {UserId}",
//                module.Name,
//                userId);

//            return await GetModuleByIdAsync(module.ModuleId);
//        }
//        catch (NotFoundException)
//        {
//            throw;
//        }
//        catch (BadRequestException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error updating dynamic module {ModuleId}", moduleDto.ModuleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Deletes a module and its table
//    /// </summary>
//    /// <param name="userId">User ID performing the deletion</param>
//    /// <param name="moduleId">Module ID to delete</param>
//    public async Task DeleteModuleAsync(int userId, int moduleId)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            var module = await _context.DynamicModules
//                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

//            if (module == null)
//            {
//                throw new NotFoundException($"Dynamic module with ID {moduleId} not found");
//            }

//            // Verificar si la tabla tiene datos
//            if (await _databaseService.TableExistsAsync(module.TableName))
//            {
//                var countQuery = $"SELECT COUNT(*) FROM [{module.TableName}]";
//                var recordCount = await _context.Database.SqlQueryRaw<int>(countQuery).FirstAsync();

//                if (recordCount > 0)
//                {
//                    throw new BadRequestException(
//                        $"Cannot delete module because the table '{module.TableName}' contains {recordCount} records. " +
//                        "Please delete all records first.");
//                }

//                // Eliminar la tabla física
//                await _databaseService.DropTableAsync(module.TableName);
//            }

//            // Eliminar el módulo (cascade eliminará los campos)
//            _context.DynamicModules.Remove(module);
//            await _context.SaveChangesAsync();

//            await transaction.CommitAsync();

//            _logger.LogInformation(
//                "Dynamic module '{ModuleName}' with table '{TableName}' deleted by user {UserId}",
//                module.Name,
//                module.TableName,
//                userId);
//        }
//        catch (NotFoundException)
//        {
//            throw;
//        }
//        catch (BadRequestException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, "Error deleting dynamic module {ModuleId}", moduleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Gets all fields for a specific module
//    /// </summary>
//    /// <param name="moduleId">Module ID</param>
//    /// <returns>List of fields</returns>
//    public async Task<List<DynamicFieldDTO>> GetModuleFieldsAsync(int moduleId)
//    {
//        try
//        {
//            var fields = await _context.DynamicFields
//                .AsNoTracking()
//                .Where(f => f.ModuleId == moduleId)
//                .OrderBy(f => f.SortOrder)
//                .ToListAsync();

//            return _mapper.Map<List<DynamicFieldDTO>>(fields);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving fields for module {ModuleId}", moduleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Gets a field by its ID
//    /// </summary>
//    /// <param name="fieldId">Field ID</param>
//    /// <returns>Field details</returns>
//    public async Task<DynamicFieldDTO> GetFieldByIdAsync(int fieldId)
//    {
//        try
//        {
//            var field = await _context.DynamicFields
//                .AsNoTracking()
//                .FirstOrDefaultAsync(f => f.FieldId == fieldId);

//            if (field == null)
//            {
//                throw new NotFoundException($"Field with ID {fieldId} not found");
//            }

//            return _mapper.Map<DynamicFieldDTO>(field);
//        }
//        catch (NotFoundException)
//        {
//            throw;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error retrieving field {FieldId}", fieldId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Adds a field to an existing module
//    /// </summary>
//    /// <param name="userId">User ID</param>
//    /// <param name="moduleId">Module ID</param>
//    /// <param name="fieldDto">Field data</param>
//    /// <returns>Created field</returns>
//    public async Task<DynamicFieldDTO> AddFieldToModuleAsync(int userId, int moduleId, AddDynamicFieldDTO fieldDto)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            var module = await _context.DynamicModules
//                .Include(m => m.DynamicFields)
//                .FirstOrDefaultAsync(m => m.ModuleId == moduleId);

//            if (module == null)
//            {
//                throw new NotFoundException($"Module with ID {moduleId} not found");
//            }

//            await ValidateFieldDefinitionAsync(fieldDto, moduleId);

//            // Verificar que no existe una columna con el mismo nombre
//            if (await _databaseService.ColumnExistsAsync(module.TableName, fieldDto.ColumnName))
//            {
//                throw new BadRequestException($"Column '{fieldDto.ColumnName}' already exists in table '{module.TableName}'");
//            }

//            var field = _mapper.Map<DynamicField>(fieldDto);
//            field.ModuleId = moduleId;
//            field.CreatedAt = DateTime.UtcNow;
//            field.CreatedByUserId = userId;

//            // Asignar orden
//            var maxOrder = module.DynamicFields.Any() ? module.DynamicFields.Max(f => f.SortOrder) : 0;
//            field.SortOrder = maxOrder + 1;

//            _context.DynamicFields.Add(field);
//            await _context.SaveChangesAsync();

//            // Agregar la columna a la tabla física
//            var fieldDtoResult = _mapper.Map<DynamicFieldDTO>(field);
//            await _databaseService.AlterTableAddColumnAsync(module.TableName, fieldDtoResult);

//            await transaction.CommitAsync();

//            _logger.LogInformation(
//                "Field '{FieldName}' added to module {ModuleId} by user {UserId}",
//                field.FieldName,
//                moduleId,
//                userId);

//            return fieldDtoResult;
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, "Error adding field to module {ModuleId}", moduleId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Updates an existing field
//    /// </summary>
//    /// <param name="userId">User ID</param>
//    /// <param name="fieldDto">Updated field data</param>
//    /// <returns>Updated field</returns>
//    public async Task<DynamicFieldDTO> UpdateFieldAsync(int userId, UpdateDynamicFieldDTO fieldDto)
//    {
//        try
//        {
//            var field = await _context.DynamicFields
//                .Include(f => f.Module)
//                .FirstOrDefaultAsync(f => f.FieldId == fieldDto.FieldId);

//            if (field == null)
//            {
//                throw new NotFoundException($"Field with ID {fieldDto.FieldId} not found");
//            }

//            await ValidateFieldDefinitionAsync(fieldDto, field.ModuleId);

//            // Solo actualizar propiedades de metadata (no estructura)
//            field.FieldName = fieldDto.Name;
//            field.DisplayName = fieldDto.DisplayName;
//            field.Description = fieldDto.Description;
//            field.ShowInList = fieldDto.ShowInList;
//            field.ShowInForm = fieldDto.ShowInForm;
//            field.SortOrder = fieldDto.SortOrder;
//            field.LookupTable = fieldDto.LookupTable;
//            field.LookupKeyColumn = fieldDto.LookupKeyColumn;
//            field.LookupDisplayColumn = fieldDto.LookupDisplayColumn;
//            field.ValidationRules = fieldDto.ValidationRules;

//            await _context.SaveChangesAsync();

//            _logger.LogInformation(
//                "Field '{FieldName}' updated by user {UserId}",
//                field.FieldName,
//                userId);

//            return _mapper.Map<DynamicFieldDTO>(field);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error updating field {FieldId}", fieldDto.FieldId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Deletes a field from a module
//    /// </summary>
//    /// <param name="userId">User ID</param>
//    /// <param name="fieldId">Field ID to delete</param>
//    public async Task DeleteFieldAsync(int userId, int fieldId)
//    {
//        using var transaction = await _context.Database.BeginTransactionAsync();

//        try
//        {
//            var field = await _context.DynamicFields
//                .Include(f => f.Module)
//                .FirstOrDefaultAsync(f => f.FieldId == fieldId);

//            if (field == null)
//            {
//                throw new NotFoundException($"Field with ID {fieldId} not found");
//            }

//            if (field.IsPrimaryKey || field.IsIdentity)
//            {
//                throw new BadRequestException("Cannot delete primary key or identity fields");
//            }

//            // Verificar si se puede eliminar la columna
//            if (!await _databaseService.CanDropColumnAsync(field.Module.TableName, field.ColumnName))
//            {
//                throw new BadRequestException($"Cannot drop column '{field.ColumnName}' from table '{field.Module.TableName}'");
//            }

//            // Eliminar la columna de la tabla física
//            await _databaseService.AlterTableDropColumnAsync(field.Module.TableName, field.ColumnName);

//            // Eliminar el campo
//            _context.DynamicFields.Remove(field);
//            await _context.SaveChangesAsync();

//            await transaction.CommitAsync();

//            _logger.LogInformation(
//                "Field '{FieldName}' deleted from module {ModuleId} by user {UserId}",
//                field.FieldName,
//                field.ModuleId,
//                userId);
//        }
//        catch (Exception ex)
//        {
//            await transaction.RollbackAsync();
//            _logger.LogError(ex, "Error deleting field {FieldId}", fieldId);
//            throw;
//        }
//    }

//    /// <summary>
//    /// Validates module definition
//    /// </summary>
//    /// <param name="moduleDto">Module to validate</param>
//    public async Task ValidateModuleDefinitionAsync(AddDynamicModuleDTO moduleDto)
//    {
//        var errors = new List<string>();

//        // Validar que tiene al menos un campo PK
//        var hasPrimaryKey = moduleDto.Fields.Any(f => f.IsPrimaryKey);
//        if (!hasPrimaryKey)
//        {
//            errors.Add("Module must have at least one primary key field");
//        }

//        // Validar que los nombres de columna son únicos
//        var duplicateColumns = moduleDto.Fields
//            .GroupBy(f => f.ColumnName.ToLower())
//            .Where(g => g.Count() > 1)
//            .Select(g => g.Key)
//            .ToList();

//        if (duplicateColumns.Any())
//        {
//            errors.Add($"Duplicate column names: {string.Join(", ", duplicateColumns)}");
//        }

//        // Validar cada campo
//        foreach (var field in moduleDto.Fields)
//        {
//            await ValidateFieldDefinitionAsync(field);
//        }

//        if (errors.Any())
//        {
//            throw new BadRequestException($"Module validation failed: {string.Join("; ", errors)}");
//        }
//    }

//    /// <summary>
//    /// Validates field definition
//    /// </summary>
//    /// <param name="fieldDto">Field to validate</param>
//    /// <param name="moduleId">Module ID (for existing module validation)</param>
//    public async Task ValidateFieldDefinitionAsync(AddDynamicFieldDTO fieldDto, int? moduleId = null)
//    {
//        var errors = new List<string>();

//        // Validaciones básicas de tipo de dato
//        switch (fieldDto.DataType.ToLower())
//        {
//            case "varchar":
//            case "nvarchar":
//                if (!fieldDto.MaxLength.HasValue || fieldDto.MaxLength <= 0)
//                {
//                    errors.Add($"Field '{fieldDto.Name}' requires a valid MaxLength for {fieldDto.DataType}");
//                }
//                break;

//            case "decimal":
//                if (!fieldDto.Precision.HasValue || fieldDto.Precision <= 0)
//                {
//                    errors.Add($"Field '{fieldDto.Name}' requires a valid Precision for decimal");
//                }
//                if (!fieldDto.Scale.HasValue || fieldDto.Scale < 0 || fieldDto.Scale > fieldDto.Precision)
//                {
//                    errors.Add($"Field '{fieldDto.Name}' requires a valid Scale for decimal (0 <= Scale <= Precision)");
//                }
//                break;
//        }

//        // Validar lookups
//        if (!string.IsNullOrEmpty(fieldDto.LookupTable))
//        {
//            if (string.IsNullOrEmpty(fieldDto.LookupKeyColumn) || string.IsNullOrEmpty(fieldDto.LookupDisplayColumn))
//            {
//                errors.Add($"Field '{fieldDto.Name}' with lookup table requires LookupKeyColumn and LookupDisplayColumn");
//            }
//            else if (!await _databaseService.TableExistsAsync(fieldDto.LookupTable))
//            {
//                errors.Add($"Lookup table '{fieldDto.LookupTable}' does not exist");
//            }
//        }

//        // Validar reglas de validación JSON
//        if (!string.IsNullOrEmpty(fieldDto.ValidationRules))
//        {
//            try
//            {
//                JsonSerializer.Deserialize<object>(fieldDto.ValidationRules);
//            }
//            catch
//            {
//                errors.Add($"Field '{fieldDto.Name}' has invalid JSON in ValidationRules");
//            }
//        }

//        if (errors.Any())
//        {
//            throw new BadRequestException($"Field validation failed: {string.Join("; ", errors)}");
//        }
//    }

//    /// <summary>
//    /// Validates module update
//    /// </summary>
//    /// <param name="moduleDto">Module update to validate</param>
//    public async Task ValidateModuleUpdateAsync(UpdateDynamicModuleDTO moduleDto)
//    {
//        // Por ahora solo validaciones básicas
//        // Las validaciones de campos se manejan por separado
//        await Task.CompletedTask;
//    }

//    /// <summary>
//    /// Checks if a table exists
//    /// </summary>
//    /// <param name="tableName">Table name</param>
//    /// <returns>True if exists</returns>
//    public async Task<bool> TableExistsAsync(string tableName)
//    {
//        return await _databaseService.TableExistsAsync(tableName);
//    }

//    /// <summary>
//    /// Checks if a module with the name exists
//    /// </summary>
//    /// <param name="name">Module name</param>
//    /// <param name="excludeId">ID to exclude from check</param>
//    /// <returns>True if exists</returns>
//    public async Task<bool> ModuleExistsAsync(string name, int? excludeId = null)
//    {
//        return await _context.DynamicModules
//            .AnyAsync(m => m.Name.ToLower() == name.ToLower() &&
//                          (!excludeId.HasValue || m.ModuleId != excludeId.Value));
//    }
//}
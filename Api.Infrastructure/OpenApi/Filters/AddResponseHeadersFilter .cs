using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.OpenApi.Filters
{
    /// <summary>
    /// Agrega ejemplos de respuestas automáticamente
    /// </summary>
    public class AddResponseHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
                operation.Responses = new OpenApiResponses();

            // Agregar respuesta 401 para endpoints que requieren autenticación
            var hasAuthorizeAttribute = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>()
                .Any() ?? false;

            if (hasAuthorizeAttribute && !operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "No autorizado - Token inválido o expirado"
                });
            }

            // Agregar respuesta 500 si no existe
            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "Error interno del servidor"
                });
            }
        }
    }

    /// <summary>
    /// Mejora la documentación de enums
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum)
                return;

            schema.Enum.Clear();

            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                schema.Enum.Add(new OpenApiString($"{Convert.ToInt64(enumValue)} - {enumValue}"));
            }
        }
    }

}

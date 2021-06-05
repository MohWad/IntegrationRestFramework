using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace APITemplate.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        private readonly ILogger _logger;

        public SwaggerDefaultValues(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Swagger");
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            try
            {
                var apiDescription = context.ApiDescription;

                operation.Deprecated |= apiDescription.IsDeprecated();

                if (operation.Parameters == null)
                {
                    return;
                }

                foreach (var parameter in operation.Parameters)
                {
                    var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                    if (parameter.Description == null)
                    {
                        parameter.Description = description.ModelMetadata?.Description;
                    }

                    if (parameter.Schema.Default == null && description.DefaultValue != null)
                    {
                        parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                    }

                    parameter.Required |= description.IsRequired;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error while applying SwaggerDefaultValues");
                throw ex;
            }
        }
    }
}

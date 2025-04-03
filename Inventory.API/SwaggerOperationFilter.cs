using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CensusFieldSurvey.API
{

    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attribute = context.MethodInfo.GetCustomAttributes(true).OfType<HttpGetAttribute>().FirstOrDefault();
            if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
            {
                operation.OperationId = attribute.Name;
            }
        }
    }
}

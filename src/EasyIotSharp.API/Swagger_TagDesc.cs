using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace EasyIotSharp
{
    /// <summary>
    /// swagger 标签描述
    /// </summary>
    public class Swagger_TagDesc : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var tags = new List<OpenApiTag>
            {
                new OpenApiTag {
                    Name = "Home",
                    Description = "<code>Swagger</code>",
                    ExternalDocs= new OpenApiExternalDocs
                    {
                        Description = ""
                    }
                }
            };

        swaggerDoc.Tags = tags;
    }
}
}
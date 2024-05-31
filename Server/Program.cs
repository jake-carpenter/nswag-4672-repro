using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg => cfg.ParameterFilter<SimplePathParameterFilter>());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Will accept IDs like /todos/1,2,3 as strings
app
    .MapGet("/todos/{ids}", (string ids) => Results.Ok(ids.Split(',')))
    .Produces<IEnumerable<string>>();

app.Run();

public class SimplePathParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        parameter.In = ParameterLocation.Path;
        parameter.Style = ParameterStyle.Simple;
        parameter.Explode = false;
        parameter.AllowEmptyValue = false;
        parameter.Schema = new OpenApiSchema
        {
            Type = "array",
            Items = new OpenApiSchema { Type = "string" }
        };
    }
}
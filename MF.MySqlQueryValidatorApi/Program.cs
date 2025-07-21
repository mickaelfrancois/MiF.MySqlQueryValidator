using Microsoft.AspNetCore.Mvc;
using MiF.MySqlQueryValidator.Analysers;
using MySqlQueryValidatorApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/analyse", ([FromBody] QueryRequest request) =>
{
    MySqlQueryAnalyzer queryAnalyzer = new();

    try
    {
        return Results.Ok(queryAnalyzer.AnalyseQuery(request.Sql));
    }
    catch (ArgumentException ex)
    {
        return Results.Problem(
            title: "Bad request",
            detail: ex.Message,
            statusCode: StatusCodes.Status400BadRequest
        );
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Internal server error",
            detail: ex.Message,
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
})
.WithName("Analyse");

app.Run();

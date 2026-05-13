using Formacio.Api;
using Formacio.Domain.Actions;
using Formacio.Domain.Actions.Matricula;
using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

var connStr = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Database=formacio;Username=formacio;Password=formacio";
builder.Services.AddDbContext<FormacioDb>(opt => opt.UseNpgsql(connStr));

builder.Services.AddScoped<SolicitarMatriculaAction>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FormacioDb>();
    await db.Database.MigrateAsync();
}

app.MapPost("/matriculas", async (SolicitacaoRequest dto, SolicitarMatriculaAction action, FormacioDb db) =>
{
    var interessado = new Interessado
    {
        Nome = dto.NomeInteressado,
        CursoDesejado = dto.CursoDesejado,
        Contato = dto.Contato
    };

    var ficha = new FichaMatricula();
    var req = new SolicitarMatriculaRequest(ficha, dto.NomeInteressado);

    try
    {
        var resultado = await action.ExecuteAsync(interessado, req);

        var solicitacao = new SolicitacaoMatricula
        {
            NomeInteressado = resultado.NomeAluno,
            CursoDesejado = resultado.Curso,
            Estado = resultado.Estado
        };

        db.Solicitacoes.Add(solicitacao);
        await db.SaveChangesAsync();

        return Results.Created($"/matriculas/{solicitacao.Id}", solicitacao);
    }
    catch (PreCondicaoNaoSatisfeitaException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
});

app.MapGet("/matriculas", async (FormacioDb db) =>
    await db.Solicitacoes.ToListAsync());

app.MapGet("/matriculas/{id}", async (string id, FormacioDb db) =>
    await db.Solicitacoes.FindAsync(id) is { } s
        ? Results.Ok(s)
        : Results.NotFound());

app.Run();

public record SolicitacaoRequest(string NomeInteressado, string CursoDesejado, string Contato = "");

public partial class Program { }

using Formacio.Api;
using Formacio.Api.Repositories;
using Formacio.Domain.Actions;
using Formacio.Domain.Actions.Matricula;
using Formacio.Domain.Actors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

var connStr = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Database=formacio;Username=formacio;Password=formacio";
builder.Services.AddDbContext<FormacioDb>(opt => opt.UseNpgsql(connStr));

builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<SolicitarMatriculaAction>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FormacioDb>();
    await db.Database.EnsureCreatedAsync();
}

app.MapPost("/matriculas", async (
    SolicitarMatriculaDto dto,
    SolicitarMatriculaAction action,
    CancellationToken ct) =>
{
    var interessado = new Interessado
    {
        Id = dto.InteressadoId,
        Nome = dto.NomeInteressado,
        CursoDesejado = dto.CursoDesejado,
        Contato = dto.Contato,
    };

    var req = new SolicitarMatriculaRequest(dto.InteressadoId);

    try
    {
        var resultado = await action.ExecuteAsync(interessado, req, ct);
        return Results.Created($"/matriculas/{resultado.MatriculaId}", resultado);
    }
    catch (PreCondicaoNaoSatisfeitaException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
});

app.MapGet("/matriculas", async (FormacioDb db, CancellationToken ct) =>
    await db.Solicitacoes.ToListAsync(ct));

app.MapGet("/matriculas/{id}", async (string id, FormacioDb db, CancellationToken ct) =>
    await db.Solicitacoes.FindAsync([id], ct) is { } s
        ? Results.Ok(s)
        : Results.NotFound());

if (app.Environment.IsDevelopment())
{
    app.MapPost("/dev/seed", async (SeedInteressadoDto dto, FormacioDb db, CancellationToken ct) =>
    {
        var fichaExistente = await db.FichasMatricula
            .FirstOrDefaultAsync(f => f.InteressadoId == dto.InteressadoId, ct);

        if (fichaExistente is not null)
            return Results.Conflict(new { erro = "Interessado já possui ficha e contrato na base de dados." });

        var ficha = new Formacio.Domain.Bodies.FichaMatricula
        {
            InteressadoId = dto.InteressadoId,
            NomeAluno = dto.NomeInteressado,
            Contato = dto.Contato,
            Curso = dto.CursoDesejado,
            Modalidade = dto.Modalidade,
            Estado = Formacio.Domain.Bodies.FichaMatriculaState.Preenchida,
            DataPreenchimento = DateTime.UtcNow,
        };

        var contrato = new Formacio.Domain.Bodies.Contrato
        {
            InteressadoId = dto.InteressadoId,
            NomeAluno = dto.NomeInteressado,
            Curso = dto.CursoDesejado,
            ValorMensalidade = dto.ValorMensalidade,
            TermosDoContrato = "Contrato de seed para testes.",
            Estado = Formacio.Domain.Bodies.ContratoState.AssinadoPorAmbos,
            DataAssinaturaInteressado = DateTime.UtcNow,
            DataAssinaturaSecretaria = DateTime.UtcNow,
        };

        db.FichasMatricula.Add(ficha);
        db.Contratos.Add(contrato);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new { ficha.InteressadoId, fichaId = ficha.Id, contratoId = contrato.Id });
    });
}

app.Run();

public record SolicitarMatriculaDto(
    string InteressadoId,
    string NomeInteressado,
    string CursoDesejado,
    string Contato = "");

public record SeedInteressadoDto(
    string InteressadoId,
    string NomeInteressado,
    string CursoDesejado,
    string Contato = "",
    string Modalidade = "Turma",
    decimal ValorMensalidade = 150m);

public partial class Program { }

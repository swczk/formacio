using Formacio.Api;
using Formacio.Api.Repositories;
using Formacio.Domain.Actions;
using Formacio.Domain.Actions.Cadastro;
using Formacio.Domain.Actions.Contratos;
using Formacio.Domain.Actions.Ficha;
using Formacio.Domain.Actions.Matriculas;
using Formacio.Domain.Actors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));

var connStr = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Database=formacio;Username=formacio;Password=formacio";
builder.Services.AddDbContext<FormacioDb>(opt => opt.UseNpgsql(connStr));

builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<IFichaRepository, FichaRepository>();
builder.Services.AddScoped<IContratoRepository, ContratoRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<CadastrarInteressadoAction>();
builder.Services.AddScoped<PreencherFichaAction>();
builder.Services.AddScoped<EfetuarContratoAction>();
builder.Services.AddScoped<MatriculaAction>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FormacioDb>();
    await db.Database.EnsureCreatedAsync();
}

// ── Interessados ─────────────────────────────────────────────────────────────

app.MapPost("/interessados", async (
    CadastrarInteressadoDto dto,
    CadastrarInteressadoAction action,
    CancellationToken ct) =>
{
    var secretaria = new Secretaria { Nome = "Sistema" };
    var req = new CadastrarInteressadoRequest(dto.Nome, dto.Contato, dto.CursoDesejado);
    try
    {
        var resultado = await action.ExecuteAsync(secretaria, req, ct);
        return Results.Created($"/interessados/{resultado.InteressadoId}", resultado);
    }
    catch (PreCondicaoNaoSatisfeitaException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
});

app.MapGet("/interessados", async (FormacioDb db, CancellationToken ct) =>
    await db.Interessados.Where(i => i.Valido).ToListAsync(ct));

app.MapGet("/interessados/{id}", async (string id, FormacioDb db, CancellationToken ct) =>
    await db.Interessados.FindAsync([id], ct) is { } i
        ? Results.Ok(i)
        : Results.NotFound());

app.MapPost("/interessados/{id}/ficha", async (
    string id,
    PreencherFichaDto dto,
    PreencherFichaAction action,
    CancellationToken ct) =>
{
    var secretaria = new Secretaria { Nome = "Sistema" };
    var req = new PreencherFichaRequest(id, dto.NomeAluno, dto.Contato, dto.Curso, dto.Modalidade);
    try
    {
        var resultado = await action.ExecuteAsync(secretaria, req, ct);
        return Results.Created($"/interessados/{id}/ficha", resultado);
    }
    catch (PreCondicaoNaoSatisfeitaException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
});

app.MapPost("/interessados/{id}/contrato", async (
    string id,
    EfetuarContratoDto dto,
    EfetuarContratoAction action,
    CancellationToken ct) =>
{
    var secretaria = new Secretaria { Nome = "Sistema" };
    var req = new EfetuarContratoRequest(id, dto.ValorMensalidade, dto.TermosDoContrato);
    try
    {
        var resultado = await action.ExecuteAsync(secretaria, req, ct);
        return Results.Created($"/interessados/{id}/contrato", resultado);
    }
    catch (PreCondicaoNaoSatisfeitaException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
});

// ── Matrículas ───────────────────────────────────────────────────────────────

app.MapPost("/matriculas", async (
    SolicitarMatriculaDto dto,
    MatriculaAction action,
    IMatriculaRepository repo,
    CancellationToken ct) =>
{
    var interessado = await repo.BuscarInteressadoAsync(dto.InteressadoId, ct);
    if (interessado is null)
        return Results.NotFound(new { erro = $"Interessado '{dto.InteressadoId}' não está cadastrado. Registe-o primeiro em POST /interessados." });

    var req = new MatriculaRequest(dto.InteressadoId);
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
    await db.Matriculas.ToListAsync(ct));

app.MapGet("/matriculas/{id}", async (string id, FormacioDb db, CancellationToken ct) =>
    await db.Matriculas.FindAsync([id], ct) is { } s
        ? Results.Ok(s)
        : Results.NotFound());

app.Run();

public record CadastrarInteressadoDto(string Nome, string Contato, string CursoDesejado);
public record PreencherFichaDto(string NomeAluno, string Contato, string Curso, string Modalidade = "Turma");
public record EfetuarContratoDto(decimal ValorMensalidade, string TermosDoContrato = "");
public record SolicitarMatriculaDto(string InteressadoId);

public partial class Program { }

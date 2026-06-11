using Formacio.Domain.Actions.Matriculas;
using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api.Repositories;

public class MatriculaRepository(FormacioDb db) : IMatriculaRepository
{
    public Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct) =>
        db.Interessados.FirstOrDefaultAsync(i => i.Id == interessadoId && i.Valido, ct);

    public Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct) =>
        db.FichasMatricula.FirstOrDefaultAsync(f => f.InteressadoId == interessadoId, ct);

    public Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct) =>
        db.Contratos.FirstOrDefaultAsync(c => c.InteressadoId == interessadoId, ct);

    public Task<bool> ExisteMatriculaAtivaAsync(string interessadoId, CancellationToken ct) =>
        db.Matriculas.AnyAsync(
            s => s.InteressadoId == interessadoId && s.Estado == MatriculaState.Activa, ct);

    public async Task<Matricula> GravarMatriculaAsync(
        Matricula matricula, CancellationToken ct)
    {
        db.Matriculas.Add(matricula);
        await db.SaveChangesAsync(ct);
        return matricula;
    }
}

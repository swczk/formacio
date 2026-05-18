using Formacio.Domain.Actions.Matricula;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api.Repositories;

public class MatriculaRepository(FormacioDb db) : IMatriculaRepository
{
    public Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct) =>
        db.FichasMatricula.FirstOrDefaultAsync(f => f.InteressadoId == interessadoId, ct);

    public Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct) =>
        db.Contratos.FirstOrDefaultAsync(c => c.InteressadoId == interessadoId, ct);

    public Task<bool> ExisteMatriculaAtivaAsync(string interessadoId, CancellationToken ct) =>
        db.Solicitacoes.AnyAsync(
            s => s.InteressadoId == interessadoId && s.Estado == SolicitacaoState.Activa, ct);

    public async Task<SolicitacaoMatricula> GravarSolicitacaoAsync(
        SolicitacaoMatricula solicitacao, CancellationToken ct)
    {
        db.Solicitacoes.Add(solicitacao);
        await db.SaveChangesAsync(ct);
        return solicitacao;
    }
}

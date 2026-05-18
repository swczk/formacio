using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Matricula;

public interface IMatriculaRepository
{
    Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct);
    Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct);
    Task<bool> ExisteMatriculaAtivaAsync(string interessadoId, CancellationToken ct);
    Task<SolicitacaoMatricula> GravarSolicitacaoAsync(SolicitacaoMatricula solicitacao, CancellationToken ct);
}

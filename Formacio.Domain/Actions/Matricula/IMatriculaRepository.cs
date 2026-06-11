using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Matriculas;

public interface IMatriculaRepository
{
    Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct);
    Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct);
    Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct);
    Task<bool> ExisteMatriculaAtivaAsync(string interessadoId, CancellationToken ct);
    Task<Matricula> GravarMatriculaAsync(Matricula matricula, CancellationToken ct);
}

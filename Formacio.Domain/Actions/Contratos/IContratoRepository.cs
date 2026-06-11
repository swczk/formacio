using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Contratos;

public interface IContratoRepository
{
    Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct);
    Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct);
    Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct);
    Task<Contrato> GravarContratoAsync(Contrato contrato, CancellationToken ct);
}

using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Ficha;

public interface IFichaRepository
{
    Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct);
    Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct);
    Task<FichaMatricula> GravarFichaAsync(FichaMatricula ficha, CancellationToken ct);
}

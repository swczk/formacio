using Formacio.Domain.Actions.Ficha;
using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api.Repositories;

public class FichaRepository(FormacioDb db) : IFichaRepository
{
    public Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct) =>
        db.Interessados.FirstOrDefaultAsync(i => i.Id == interessadoId && i.Valido, ct);

    public Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct) =>
        db.FichasMatricula.FirstOrDefaultAsync(f => f.InteressadoId == interessadoId, ct);

    public async Task<FichaMatricula> GravarFichaAsync(FichaMatricula ficha, CancellationToken ct)
    {
        db.FichasMatricula.Add(ficha);
        await db.SaveChangesAsync(ct);
        return ficha;
    }
}

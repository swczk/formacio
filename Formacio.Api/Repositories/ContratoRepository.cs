using Formacio.Domain.Actions.Contratos;
using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api.Repositories;

public class ContratoRepository(FormacioDb db) : IContratoRepository
{
    public Task<Interessado?> BuscarInteressadoAsync(string interessadoId, CancellationToken ct) =>
        db.Interessados.FirstOrDefaultAsync(i => i.Id == interessadoId && i.Valido, ct);

    public Task<FichaMatricula?> BuscarFichaAsync(string interessadoId, CancellationToken ct) =>
        db.FichasMatricula.FirstOrDefaultAsync(f => f.InteressadoId == interessadoId, ct);

    public Task<Contrato?> BuscarContratoAsync(string interessadoId, CancellationToken ct) =>
        db.Contratos.FirstOrDefaultAsync(c => c.InteressadoId == interessadoId, ct);

    public async Task<Contrato> GravarContratoAsync(Contrato contrato, CancellationToken ct)
    {
        db.Contratos.Add(contrato);
        await db.SaveChangesAsync(ct);
        return contrato;
    }
}

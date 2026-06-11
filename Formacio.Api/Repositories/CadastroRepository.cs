using Formacio.Domain.Actions.Cadastro;
using Formacio.Domain.Actors;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api.Repositories;

public class CadastroRepository(FormacioDb db) : ICadastroRepository
{
    public Task<bool> ExisteContatoAsync(string contato, CancellationToken ct) =>
        db.Interessados.AnyAsync(i => i.Contato == contato, ct);

    public async Task<Interessado> GravarInteressadoAsync(Interessado interessado, CancellationToken ct)
    {
        db.Interessados.Add(interessado);
        await db.SaveChangesAsync(ct);
        return interessado;
    }
}

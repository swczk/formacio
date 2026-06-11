using Formacio.Domain.Actors;

namespace Formacio.Domain.Actions.Cadastro;

public interface ICadastroRepository
{
    Task<bool> ExisteContatoAsync(string contato, CancellationToken ct);
    Task<Interessado> GravarInteressadoAsync(Interessado interessado, CancellationToken ct);
}

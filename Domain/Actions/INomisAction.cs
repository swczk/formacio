using Formacio.Domain.Actors;

namespace Formacio.Domain.Actions;

public record ActionResult(bool Ok, string? Erro = null)
{
   public static ActionResult Sucesso() => new(true);
   public static ActionResult Falha(string erro) => new(false, erro);
}

public interface INomisAction<TActor, TRequest, TResponse>
    where TActor : IActor
{
   Task<ActionResult> CanExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default);
   Task<TResponse> ExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default);
}

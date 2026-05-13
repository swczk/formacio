using Formacio.Domain.Actors;

namespace Formacio.Domain.Actions;

public record ActionResult(bool Ok, string? Erro = null)
{
   public static ActionResult Sucesso() => new(true);
   public static ActionResult Falha(string erro) => new(false, erro);
}

public sealed class PreCondicaoNaoSatisfeitaException(string erro)
    : InvalidOperationException(erro);

public interface INomisAction<TActor, TRequest, TResponse>
    where TActor : IActor
{
   Task<ActionResult> CanExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default);
   Task<TResponse> ExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default);
}

public abstract class NomisAction<TActor, TRequest, TResponse>
    : INomisAction<TActor, TRequest, TResponse>
    where TActor : IActor
{
   public abstract Task<ActionResult> CanExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default);

   protected abstract Task<TResponse> ExecutarAsync(TActor ator, TRequest request, CancellationToken ct);

   public async Task<TResponse> ExecuteAsync(TActor ator, TRequest request, CancellationToken ct = default)
   {
      var resultado = await CanExecuteAsync(ator, request, ct);
      if (!resultado.Ok)
         throw new PreCondicaoNaoSatisfeitaException(resultado.Erro!);
      return await ExecutarAsync(ator, request, ct);
   }
}

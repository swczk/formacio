using Formacio.Domain.Actors;

namespace Formacio.Domain.Actions.Cadastro;

public record CadastrarInteressadoRequest(string Nome, string Contato, string CursoDesejado);
public record CadastrarInteressadoResponse(string InteressadoId, string Nome, string CursoDesejado);

public class CadastrarInteressadoAction(ICadastroRepository repo)
    : NomisAction<Secretaria, CadastrarInteressadoRequest, CadastrarInteressadoResponse>
{
    public override async Task<ActionResult> CanExecuteAsync(
        Secretaria ator,
        CadastrarInteressadoRequest req,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(req.Nome))
            return ActionResult.Falha("O nome do interessado é obrigatório.");

        if (string.IsNullOrWhiteSpace(req.Contato))
            return ActionResult.Falha("O contacto (telefone ou e-mail) é obrigatório.");

        if (string.IsNullOrWhiteSpace(req.CursoDesejado))
            return ActionResult.Falha("O curso desejado é obrigatório.");

        if (await repo.ExisteContatoAsync(req.Contato, ct))
            return ActionResult.Falha($"Já existe um interessado registado com o contacto '{req.Contato}'.");

        return ActionResult.Sucesso();
    }

    protected override async Task<CadastrarInteressadoResponse> ExecutarAsync(
        Secretaria ator,
        CadastrarInteressadoRequest req,
        CancellationToken ct)
    {
        var interessado = new Interessado
        {
            Nome = req.Nome,
            Contato = req.Contato,
            CursoDesejado = req.CursoDesejado,
            Valido = true,
        };

        var gravado = await repo.GravarInteressadoAsync(interessado, ct);
        return new CadastrarInteressadoResponse(gravado.Id, gravado.Nome, gravado.CursoDesejado);
    }
}

using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Ficha;

public record PreencherFichaRequest(
    string InteressadoId,
    string NomeAluno,
    string Contato,
    string Curso,
    string Modalidade);

public record PreencherFichaResponse(string FichaId, string NomeAluno, string Curso);

public class PreencherFichaAction(IFichaRepository repo)
    : NomisAction<Secretaria, PreencherFichaRequest, PreencherFichaResponse>
{
    public override async Task<ActionResult> CanExecuteAsync(
        Secretaria ator,
        PreencherFichaRequest req,
        CancellationToken ct = default)
    {
        var interessado = await repo.BuscarInteressadoAsync(req.InteressadoId, ct);
        if (interessado is null || !interessado.Valido)
            return ActionResult.Falha("Interessado não está cadastrado ou não está activo.");

        if (string.IsNullOrWhiteSpace(req.NomeAluno))
            return ActionResult.Falha("O nome do aluno é obrigatório.");

        if (string.IsNullOrWhiteSpace(req.Curso))
            return ActionResult.Falha("O curso é obrigatório.");

        var fichaExistente = await repo.BuscarFichaAsync(req.InteressadoId, ct);
        if (fichaExistente is not null && fichaExistente.Valido)
            return ActionResult.Falha("Interessado já possui ficha de matrícula preenchida.");

        return ActionResult.Sucesso();
    }

    protected override async Task<PreencherFichaResponse> ExecutarAsync(
        Secretaria ator,
        PreencherFichaRequest req,
        CancellationToken ct)
    {
        var ficha = new FichaMatricula
        {
            InteressadoId = req.InteressadoId,
            NomeAluno = req.NomeAluno,
            Contato = req.Contato,
            Curso = req.Curso,
            Modalidade = req.Modalidade,
            Estado = FichaMatriculaState.Preenchida,
            DataPreenchimento = DateTime.UtcNow,
        };

        var gravada = await repo.GravarFichaAsync(ficha, ct);
        return new PreencherFichaResponse(gravada.Id, gravada.NomeAluno, gravada.Curso);
    }
}

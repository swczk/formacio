using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Matricula;

public record SolicitarMatriculaRequest(string InteressadoId);
public record SolicitarMatriculaResponse(string MatriculaId, string NomeAluno, string Curso);

public class SolicitarMatriculaAction(IMatriculaRepository repo)
    : NomisAction<Interessado, SolicitarMatriculaRequest, SolicitarMatriculaResponse>
{
    public override async Task<ActionResult> CanExecuteAsync(
        Interessado ator,
        SolicitarMatriculaRequest req,
        CancellationToken ct = default)
    {
        var ficha = await repo.BuscarFichaAsync(ator.Id, ct);

        if (ficha is null)
            return ActionResult.Falha(
                "Interessado não possui ficha de matrícula. A secretária deve preencher a ficha primeiro (acção 4).");

        if (ficha.Estado != FichaMatriculaState.Preenchida)
            return ActionResult.Falha(
                $"A ficha de matrícula está em estado '{ficha.Estado}'. Deve estar Preenchida.");

        var contrato = await repo.BuscarContratoAsync(ator.Id, ct);

        if (contrato is null)
            return ActionResult.Falha(
                "Interessado não possui contrato. O contrato deve ser assinado por ambas as partes (acções 5a e 5b).");

        if (contrato.Estado != ContratoState.AssinadoPorAmbos)
            return ActionResult.Falha(
                $"O contrato está em estado '{contrato.Estado}'. Deve estar AssinadoPorAmbos.");

        if (await repo.ExisteMatriculaAtivaAsync(ator.Id, ct))
            return ActionResult.Falha("Interessado já possui matrícula activa.");

        return ActionResult.Sucesso();
    }

    protected override async Task<SolicitarMatriculaResponse> ExecutarAsync(
        Interessado ator,
        SolicitarMatriculaRequest req,
        CancellationToken ct)
    {
        var ficha = await repo.BuscarFichaAsync(ator.Id, ct);

        var solicitacao = new SolicitacaoMatricula
        {
            InteressadoId = ator.Id,
            NomeInteressado = ficha!.NomeAluno,
            CursoDesejado = ficha.Curso,
            Estado = SolicitacaoState.Activa,
        };

        var gravada = await repo.GravarSolicitacaoAsync(solicitacao, ct);
        return new SolicitarMatriculaResponse(gravada.Id, gravada.NomeInteressado, gravada.CursoDesejado);
    }
}

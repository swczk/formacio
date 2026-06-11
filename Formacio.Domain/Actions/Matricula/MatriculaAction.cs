using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Matriculas;

public record MatriculaRequest(string InteressadoId);
public record MatriculaResponse(string MatriculaId, string NomeAluno, string Curso);

public class MatriculaAction(IMatriculaRepository repo)
    : NomisAction<Interessado, MatriculaRequest, MatriculaResponse>
{
    public override async Task<ActionResult> CanExecuteAsync(
        Interessado ator,
        MatriculaRequest req,
        CancellationToken ct = default)
    {
        var ficha = await repo.BuscarFichaAsync(ator.Id, ct);

        if (ficha is null)
            return ActionResult.Falha(
                "Interessado não possui ficha de matrícula. A secretária deve preencher a ficha primeiro (acção 4).");

        if (!ficha.Valido)
            return ActionResult.Falha(
                "A ficha de matrícula não está válida. A secretária deve preenchê-la primeiro (acção 4).");

        var contrato = await repo.BuscarContratoAsync(ator.Id, ct);

        if (contrato is null)
            return ActionResult.Falha(
                "Interessado não possui contrato. O contrato deve ser assinado por ambas as partes (acções 5a e 5b).");

        if (!contrato.Valido)
            return ActionResult.Falha(
                "O contrato de prestação de serviços não está válido. Deve ser assinado por ambas as partes (acções 5a e 5b).");

        if (await repo.ExisteMatriculaAtivaAsync(ator.Id, ct))
            return ActionResult.Falha("Interessado já possui matrícula activa.");

        return ActionResult.Sucesso();
    }

    protected override async Task<MatriculaResponse> ExecutarAsync(
        Interessado ator,
        MatriculaRequest req,
        CancellationToken ct)
    {
        var ficha = await repo.BuscarFichaAsync(ator.Id, ct);

        var matricula = new Matricula
        {
            InteressadoId = ator.Id,
            NomeInteressado = ficha!.NomeAluno,
            CursoDesejado = ficha.Curso,
            Estado = MatriculaState.Activa,
        };

        var gravada = await repo.GravarMatriculaAsync(matricula, ct);
        return new MatriculaResponse(gravada.Id, gravada.NomeInteressado, gravada.CursoDesejado);
    }
}

using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Matricula;

public record SolicitarMatriculaRequest(FichaMatricula Ficha, string NomeInteressado);

public class SolicitarMatriculaAction
    : NomisAction<Interessado, SolicitarMatriculaRequest, FichaMatricula>
{
    public override Task<ActionResult> CanExecuteAsync(
        Interessado ator,
        SolicitarMatriculaRequest req,
        CancellationToken ct = default)
    {
        if (req.Ficha.Estado != FichaMatriculaState.EmBranco)
            return Task.FromResult(ActionResult.Falha("Já existe uma ficha em curso para este interessado."));

        if (string.IsNullOrWhiteSpace(ator.CursoDesejado))
            return Task.FromResult(ActionResult.Falha("Interessado deve indicar um curso antes de solicitar matrícula."));

        return Task.FromResult(ActionResult.Sucesso());
    }

    protected override Task<FichaMatricula> ExecutarAsync(
        Interessado ator,
        SolicitarMatriculaRequest req,
        CancellationToken ct)
    {
        req.Ficha.NomeAluno = ator.Nome;
        req.Ficha.Curso = ator.CursoDesejado;
        return Task.FromResult(req.Ficha);
    }
}

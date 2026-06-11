using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;

namespace Formacio.Domain.Actions.Contratos;

public record EfetuarContratoRequest(
    string InteressadoId,
    decimal ValorMensalidade,
    string TermosDoContrato = "");

public record EfetuarContratoResponse(
    string ContratoId,
    string NomeAluno,
    string Curso,
    decimal ValorMensalidade);

public class EfetuarContratoAction(IContratoRepository repo)
    : NomisAction<Secretaria, EfetuarContratoRequest, EfetuarContratoResponse>
{
    public override async Task<ActionResult> CanExecuteAsync(
        Secretaria ator,
        EfetuarContratoRequest req,
        CancellationToken ct = default)
    {
        var interessado = await repo.BuscarInteressadoAsync(req.InteressadoId, ct);
        if (interessado is null || !interessado.Valido)
            return ActionResult.Falha("Interessado não está cadastrado ou não está activo.");

        var ficha = await repo.BuscarFichaAsync(req.InteressadoId, ct);
        if (ficha is null || !ficha.Valido)
            return ActionResult.Falha("É necessário preencher a ficha de matrícula antes de efetuar o contrato.");

        var contratoExistente = await repo.BuscarContratoAsync(req.InteressadoId, ct);
        if (contratoExistente is not null && contratoExistente.Valido)
            return ActionResult.Falha("Interessado já possui contrato de prestação de serviços assinado.");

        if (req.ValorMensalidade <= 0)
            return ActionResult.Falha("O valor da mensalidade deve ser superior a zero.");

        return ActionResult.Sucesso();
    }

    protected override async Task<EfetuarContratoResponse> ExecutarAsync(
        Secretaria ator,
        EfetuarContratoRequest req,
        CancellationToken ct)
    {
        var ficha = await repo.BuscarFichaAsync(req.InteressadoId, ct);
        var agora = DateTime.UtcNow;

        var contrato = new Contrato
        {
            InteressadoId = req.InteressadoId,
            NomeAluno = ficha!.NomeAluno,
            Curso = ficha.Curso,
            ValorMensalidade = req.ValorMensalidade,
            TermosDoContrato = req.TermosDoContrato,
            Estado = ContratoState.AssinadoPorAmbos,
            DataAssinaturaInteressado = agora,
            DataAssinaturaSecretaria = agora,
        };

        var gravado = await repo.GravarContratoAsync(contrato, ct);
        return new EfetuarContratoResponse(gravado.Id, gravado.NomeAluno, gravado.Curso, gravado.ValorMensalidade);
    }
}

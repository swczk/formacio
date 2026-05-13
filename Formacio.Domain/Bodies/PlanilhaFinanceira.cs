namespace Formacio.Domain.Bodies;

public class PlanilhaFinanceira
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public PlanilhaFinanceiraState Estado { get; set; } = PlanilhaFinanceiraState.EmUso;

   public List<RegistroPagamento> Registros { get; set; } = new();
}

public record RegistroPagamento(
    string AlunoId,
    string NomeAluno,
    decimal Valor,
    string MesReferencia,
    string FormaPagamento,
    DateTime DataPagamento
);

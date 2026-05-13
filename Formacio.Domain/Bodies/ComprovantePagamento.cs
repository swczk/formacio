namespace Formacio.Domain.Bodies;

public class ComprovantePagamento
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public ComprovanteState Estado { get; set; } = ComprovanteState.NaoGerado;

   public string NomeAluno { get; set; } = "";
   public decimal Valor { get; set; }
   public string MesReferencia { get; set; } = "";
   public string FormaPagamento { get; set; } = ""; // "Dinheiro", "PIX", "Cartão"
   public DateTime DataEmissao { get; set; }
}

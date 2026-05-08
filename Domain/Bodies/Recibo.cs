namespace Formacio.Domain.Bodies;

public class Recibo
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public ReciboState Estado { get; set; } = ReciboState.EmBranco;

   public string InstrutorId { get; set; } = "";
   public string NomeInstrutor { get; set; } = "";
   public decimal ValorRecebido { get; set; }
   public int QuantidadeAulas { get; set; }
   public string Periodo { get; set; } = ""; // ex: "Janeiro/2025"
   public DateTime DataAssinatura { get; set; }
}

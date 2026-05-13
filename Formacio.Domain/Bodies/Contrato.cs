namespace Formacio.Domain.Bodies;

public class Contrato
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public ContratoState Estado { get; set; } = ContratoState.EmBranco;

   public string NomeAluno { get; set; } = "";
   public string Curso { get; set; } = "";
   public decimal ValorMensalidade { get; set; }
   public string TermosDoContrato { get; set; } = "";
   public DateTime DataAssinaturaInteressado { get; set; }
   public DateTime DataAssinaturaSecretaria { get; set; }
}

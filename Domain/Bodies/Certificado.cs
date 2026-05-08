namespace Formacio.Domain.Bodies;

public class Certificado
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public CertificadoState Estado { get; set; } = CertificadoState.EmBranco;

   public string AlunoId { get; set; } = "";
   public string NomeAluno { get; set; } = "";
   public string Curso { get; set; } = "";
   public DateTime DataConclusao { get; set; }
   public DateTime DataEmissao { get; set; }
   public DateTime DataAssinaturaInstrutor { get; set; }
   public DateTime DataAssinaturaDono { get; set; }
}

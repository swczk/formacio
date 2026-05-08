namespace Formacio.Domain.Bodies;

public class PlanilhaPresenca
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public PlanilhaPresencaState Estado { get; set; } = PlanilhaPresencaState.EmBranco;

   public DateTime DataAula { get; set; }
   public string InstrutorId { get; set; } = "";
   public string ConteudoMinistrado { get; set; } = "";
   public bool PresencaInstrutorRegistrada { get; set; } = false;
   public List<RegistroPresencaAluno> PresencasAlunos { get; set; } = new();
}

public record RegistroPresencaAluno(string AlunoId, string NomeAluno, bool Presente);

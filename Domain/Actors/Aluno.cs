namespace Formacio.Domain.Actors;

public class Aluno : IActor
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public string Role => "Aluno";

   public string Nome { get; init; } = "";
   public string Contato { get; init; } = "";
   public string CursoMatriculado { get; init; } = "";
   public string Modalidade { get; init; } = ""; // "Turma" ou "Particular"
   public DateTime DataMatricula { get; init; }
   public bool MensalidadeEmDia { get; set; } = true;
   public bool CursoConcluido { get; set; } = false;
}

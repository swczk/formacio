namespace Formacio.Domain.Bodies;

public class FichaMatricula
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public FichaMatriculaState Estado { get; set; } = FichaMatriculaState.EmBranco;

   // Informações registradas durante a ação 4
   public string NomeAluno { get; set; } = "";
   public string Contato { get; set; } = "";
   public string Curso { get; set; } = "";
   public string Modalidade { get; set; } = ""; // "Turma" ou "Particular"
   public DateTime DataPreenchimento { get; set; }
}

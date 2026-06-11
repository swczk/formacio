namespace Formacio.Domain.Actors;

public class Interessado : IActor
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public string Role => "Interessado";

   public string Nome { get; init; } = "";
   public string Contato { get; init; } = "";   // telefone ou e-mail
   public string CursoDesejado { get; init; } = "";
   public bool Valido { get; set; } = true;
}

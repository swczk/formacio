namespace Formacio.Domain.Actors;

public class Instrutor : IActor
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public string Role => "Instrutor";

   public string Nome { get; init; } = "";
   public List<string> CursosQueEnsina { get; init; } = new();
}

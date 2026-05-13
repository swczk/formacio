namespace Formacio.Domain.Actors;

public class Secretaria : IActor
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public string Role => "Secretaria";

   public string Nome { get; init; } = "";
}

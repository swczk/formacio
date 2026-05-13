namespace Formacio.Domain.Actors;

public class Dono : IActor
{
   public string Id { get; init; } = Guid.NewGuid().ToString();
   public string Role => "Dono";

   public string Nome { get; init; } = "";
}

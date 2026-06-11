using System.ComponentModel.DataAnnotations.Schema;

namespace Formacio.Domain.Bodies;

public class Matricula
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string InteressadoId { get; set; } = "";
    public string NomeInteressado { get; set; } = "";
    public string CursoDesejado { get; set; } = "";
    public MatriculaState Estado { get; set; } = MatriculaState.Pendente;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public bool Valido => Estado == MatriculaState.Activa;
}

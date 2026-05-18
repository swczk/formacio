namespace Formacio.Domain.Bodies;

public class SolicitacaoMatricula
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string InteressadoId { get; set; } = "";
    public string NomeInteressado { get; set; } = "";
    public string CursoDesejado { get; set; } = "";
    public SolicitacaoState Estado { get; set; } = SolicitacaoState.Pendente;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

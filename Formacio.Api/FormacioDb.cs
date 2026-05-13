using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api;

public class FormacioDb(DbContextOptions<FormacioDb> opts) : DbContext(opts)
{
    public DbSet<SolicitacaoMatricula> Solicitacoes => Set<SolicitacaoMatricula>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<SolicitacaoMatricula>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
        });
    }
}

public class SolicitacaoMatricula
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string NomeInteressado { get; set; } = "";
    public string CursoDesejado { get; set; } = "";
    public FichaMatriculaState Estado { get; set; } = FichaMatriculaState.EmBranco;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

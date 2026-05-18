using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api;

public class FormacioDb(DbContextOptions<FormacioDb> opts) : DbContext(opts)
{
    public DbSet<SolicitacaoMatricula> Solicitacoes    => Set<SolicitacaoMatricula>();
    public DbSet<FichaMatricula>       FichasMatricula => Set<FichaMatricula>();
    public DbSet<Contrato>             Contratos       => Set<Contrato>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<SolicitacaoMatricula>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
        });

        m.Entity<FichaMatricula>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasIndex(x => x.InteressadoId);
        });

        m.Entity<Contrato>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasIndex(x => x.InteressadoId);
        });
    }
}

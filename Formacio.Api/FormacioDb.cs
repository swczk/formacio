using Formacio.Domain.Actors;
using Formacio.Domain.Bodies;
using Microsoft.EntityFrameworkCore;

namespace Formacio.Api;

public class FormacioDb(DbContextOptions<FormacioDb> opts) : DbContext(opts)
{
    public DbSet<Interessado>         Interessados    => Set<Interessado>();
    public DbSet<Matricula>           Matriculas      => Set<Matricula>();
    public DbSet<FichaMatricula>      FichasMatricula => Set<FichaMatricula>();
    public DbSet<Contrato>            Contratos       => Set<Contrato>();

    protected override void OnModelCreating(ModelBuilder m)
    {
        m.Entity<Interessado>(e =>
        {
            e.HasKey(x => x.Id);
            e.Ignore(x => x.Role);
            e.HasIndex(x => x.Contato);
        });

        m.Entity<Matricula>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.Ignore(x => x.Valido);
        });

        m.Entity<FichaMatricula>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasIndex(x => x.InteressadoId);
            e.Ignore(x => x.Valido);
        });

        m.Entity<Contrato>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Estado).HasConversion<string>();
            e.HasIndex(x => x.InteressadoId);
            e.Ignore(x => x.Valido);
        });
    }
}

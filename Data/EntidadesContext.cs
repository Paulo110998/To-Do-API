using Microsoft.EntityFrameworkCore;
using TO_DO___API.Models;

namespace TO_DO___API.Data;

public class EntidadesContext : DbContext
{

    public EntidadesContext(DbContextOptions<EntidadesContext> opts)
        : base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Listas>().ToTable("listas"); // Nome da tabela 'Listas' como 'listas'
        modelBuilder.Entity<Tarefas>().ToTable("tarefas"); // Nome da tabela 'Tarefas' como 'tarefas'
        modelBuilder.Entity<Anotacoes>().ToTable("anotacoes"); // Nome da tabela 'Anotacoes' como 'anotacoes'
        modelBuilder.Entity<MembrosFamilia>().ToTable("membrosfamilia"); // Nome da tabela 'MembrosFamilia' como 'membrosfamilia'
        modelBuilder.Entity<Familia>().ToTable("familia"); // Nome da tabela 'Familia' como 'familia'
        modelBuilder.Entity<GeradorSenhas>().ToTable("geradorsenhas"); // Nome da tabela 'GeradorSenhas' como 'geradorsenhas'

        modelBuilder.Entity<Tarefas>()
            .Property(e => e.Repeticao)
            .HasConversion<string>();

        modelBuilder.Entity<Listas>()
            .Property(e => e.Duplicacao)
            .HasConversion<string>();

        modelBuilder.Entity<Anotacoes>()
            .Property(e => e.TipoAnotacao)
            .HasConversion<string>();

        modelBuilder.Entity<MembrosFamilia>()
            .Property(e => e.VinculoFamilia)
            .HasConversion<string>();
    }

    public DbSet<Listas> Listas { get; set; }

    public DbSet<Tarefas> Tarefas { get; set; }

    public DbSet<Anotacoes> Anotacoes { get; set; }

    public DbSet<Familia> Familia { get; set; }

    public DbSet<MembrosFamilia> MembrosFamilia { get; set; }

    public DbSet<GeradorSenhas> GeradorSenhas { get; set; }


}
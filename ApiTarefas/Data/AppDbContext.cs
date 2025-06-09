using Microsoft.EntityFrameworkCore;
using TarefasApi.Models; // Importa o namespace dos modelos

namespace TarefasApi.Data; // Namespace para acesso a dados

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>().ToTable("Tarefas");

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.Property(t => t.Titulo).IsRequired();
            entity.Property(t => t.Titulo).HasMaxLength(100);
            entity.Property(t => t.Descricao).HasMaxLength(500);
        });
    }
}
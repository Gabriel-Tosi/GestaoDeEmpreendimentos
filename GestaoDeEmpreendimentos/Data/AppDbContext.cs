using GestaoDeEmpreendimentos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEmpreendimentos.Data
{
    // DbContext da aplicação com configurações do EF Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet de empreendimentos
        public DbSet<Empreendimento> Empreendimentos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garante índice único no CNPJ
            modelBuilder.Entity<Empreendimento>()
                .HasIndex(e => e.Cnpj)
                .IsUnique();
        }
    }
}

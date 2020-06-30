using CadastroUsuarios.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CadastroUsuarios.Infra.Data
{
    public class PrincipalContext : DbContext
    {
        public PrincipalContext(DbContextOptions<PrincipalContext> options)
          : base(options)
        { }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().Property(t => t.Id).IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Usuario>().Property(t => t.Nome).IsRequired().HasColumnType("VARCHAR(100)");
            modelBuilder.Entity<Usuario>().Property(t => t.EMail).IsRequired().HasColumnType("VARCHAR(200)");
            modelBuilder.Entity<Usuario>().Property(t => t.Senha).IsRequired().HasColumnType("VARCHAR(200)");
        }

    }
}

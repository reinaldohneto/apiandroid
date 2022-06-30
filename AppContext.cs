using AppAndroid.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppAndroid;

public class AppContext : IdentityDbContext
{
    public DbSet<Localizacao> Localizacoes { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    
    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Localizacao>()
            .HasKey(l => l.Id);

        builder.Entity<Localizacao>()
            .HasOne(u => u.Usuario)
            .WithMany(l => l.Localizacoes)
            .HasForeignKey(t => t.UsuarioId);

        builder.Entity<Grupo>()
            .HasKey(g => g.Id);

        builder.Entity<Grupo>()
            .HasMany(t => t.Usuarios)
            .WithOne(t => t.Grupo)
            .HasForeignKey(g => g.GrupoId);
    }
}
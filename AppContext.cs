using AppAndroid.Model;
using Microsoft.EntityFrameworkCore;

namespace AppAndroid;

public class AppContext : DbContext
{
    public DbSet<Localizacao?> Localizacoes { get; set; }
    
    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Localizacao>()
            .HasKey(l => l.Id);
    }

}
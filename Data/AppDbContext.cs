using DSW_3P_PR01.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR01.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Evento> Eventos => Set<Evento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Evita que Npgsql exija DateTime con Kind=Utc (timestamptz).
        modelBuilder.Entity<Evento>()
            .Property(e => e.Fecha)
            .HasColumnType("timestamp without time zone");
    }
}

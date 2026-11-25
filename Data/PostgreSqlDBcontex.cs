using DSW_3P_PR01.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR01.Data;

public class PostgreSqlDbContext : DbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options)
        : base(options) { }

    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Mision> Misiones => Set<Mision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>()
            .Property(e => e.Fecha)
            .HasColumnType("timestamp without time zone");
    }
}

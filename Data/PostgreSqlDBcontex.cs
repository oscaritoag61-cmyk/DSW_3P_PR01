using DSW_3P_PR01.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR01.Data;

public class PostgreSqlDbContext : DbContext
{
    public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options)
        : base(options) { }

    // Eventos vive en AppDbContext; este contexto solo maneja Misiones
    // para que ambos puedan compartir la misma base de datos.
    public DbSet<Mision> Misiones => Set<Mision>();
}

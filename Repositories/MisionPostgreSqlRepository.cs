using DSW_3P_PR01.Data;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

// Persistencia de misiones en PostgreSQL con Entity Framework Core.
public class MisionPostgreSqlRepository : IMisionRepository
{
    private readonly PostgreSqlDbContext _context;

    public MisionPostgreSqlRepository(PostgreSqlDbContext context)
    {
        _context = context;
    }

    public List<Mision> ObtenerTodos()
    {
        return _context.Misiones.ToList();
    }

    public Mision? ObtenerPorId(int id)
    {
        return _context.Misiones.Find(id);
    }

    public Mision Agregar(Mision mision)
    {
        mision.Id = 0;
        _context.Misiones.Add(mision);
        _context.SaveChanges();
        return mision;
    }

    public bool Actualizar(int id, Mision mision)
    {
        var existente = _context.Misiones.Find(id);
        if (existente == null)
            return false;

        existente.Nombre = mision.Nombre;
        existente.Dificultad = mision.Dificultad;
        existente.RecompensaXP = mision.RecompensaXP;
        existente.Completada = mision.Completada;

        _context.SaveChanges();
        return true;
    }

    public bool Eliminar(int id)
    {
        var mision = _context.Misiones.Find(id);
        if (mision == null)
            return false;

        _context.Misiones.Remove(mision);
        _context.SaveChanges();
        return true;
    }
}

using DSW_3P_PR01.Data;
using DSW_3P_PR01.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR01.Repositories;

// Segunda versión: persistencia en SQL Server con Entity Framework Core.
// Cumple exactamente el mismo contrato IEventoRepository que la versión JSON.
public class EventoSqlRepository : IEventoRepository
{
    private readonly AppDbContext _context;

    public EventoSqlRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Evento> GetAll() =>
        _context.Eventos.AsNoTracking().OrderBy(e => e.Id).ToList();

    public Evento? GetById(int id) =>
        _context.Eventos.AsNoTracking().FirstOrDefault(e => e.Id == id);

    public Evento Add(Evento evento)
    {
        _context.Eventos.Add(evento);
        _context.SaveChanges();
        return evento;
    }

    public bool Update(Evento evento)
    {
        var existente = _context.Eventos.Find(evento.Id);
        if (existente is null) return false;

        _context.Entry(existente).CurrentValues.SetValues(evento);
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var existente = _context.Eventos.Find(id);
        if (existente is null) return false;

        _context.Eventos.Remove(existente);
        _context.SaveChanges();
        return true;
    }
}

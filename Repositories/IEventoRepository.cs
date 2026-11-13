using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

// Contrato estable: el Service depende de esta interfaz, no de una implementación.
public interface IEventoRepository
{
    IEnumerable<Evento> GetAll();
    Evento? GetById(int id);
    Evento Add(Evento evento);
    bool Update(Evento evento);
    bool Delete(int id);
}

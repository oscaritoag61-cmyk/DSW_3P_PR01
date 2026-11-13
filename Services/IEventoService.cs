using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Services;

public interface IEventoService
{
    IEnumerable<Evento> ObtenerTodos();
    Evento? ObtenerPorId(int id);
    Evento Crear(Evento evento);
    bool Actualizar(int id, Evento evento);
    bool Eliminar(int id);
}

using DSW_3P_PR01.Models;
using DSW_3P_PR01.Repositories;

namespace DSW_3P_PR01.Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _repository;

    public EventoService(IEventoRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Evento> ObtenerTodos() => _repository.GetAll();

    public Evento? ObtenerPorId(int id) => _repository.GetById(id);

    public Evento Crear(Evento evento)
    {
        Validar(evento);
        evento.Id = 0; // el Id lo asigna el almacenamiento
        return _repository.Add(evento);
    }

    public bool Actualizar(int id, Evento evento)
    {
        Validar(evento);
        evento.Id = id;
        return _repository.Update(evento);
    }

    public bool Eliminar(int id) => _repository.Delete(id);

    // Reglas de negocio: nombre obligatorio, cupo > 0 y fecha válida.
    private static void Validar(Evento evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (evento.CupoMaximo <= 0)
            throw new ArgumentException("El cupo máximo debe ser mayor que cero.");

        if (evento.Fecha == default)
            throw new ArgumentException("La fecha no es válida.");
    }
}

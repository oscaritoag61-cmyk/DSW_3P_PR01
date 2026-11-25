using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

public interface IMisionRepository
{
    List<Mision> ObtenerTodos();
    Mision? ObtenerPorId(int id);
    Mision Agregar(Mision mision);
    bool Actualizar(int id, Mision mision);
    bool Eliminar(int id);
}

namespace QuestLog.Services;

/// <summary>Se rompió una regla de negocio o los datos son inválidos (HTTP 400).</summary>
public class ValidacionException : Exception
{
    public ValidacionException(string mensaje) : base(mensaje) { }
}

/// <summary>La misión solicitada no existe (HTTP 404).</summary>
public class MisionNoEncontradaException : Exception
{
    public MisionNoEncontradaException(int id)
        : base($"No existe una misión con id {id}.") { }
}

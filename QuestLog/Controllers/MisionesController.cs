using Microsoft.AspNetCore.Mvc;
using QuestLog.Models;
using QuestLog.Services;

namespace QuestLog.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MisionesController : ControllerBase
{
    private readonly IMisionService _servicio;

    public MisionesController(IMisionService servicio)
    {
        _servicio = servicio;
    }

    /// <summary>GET /api/misiones — 200 con la lista (vacía si no hay misiones).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Mision>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Mision>>> ObtenerTodas()
    {
        return Ok(await _servicio.ObtenerTodasAsync());
    }

    /// <summary>GET /api/misiones/{id} — 200, o 404 si no existe.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Mision>> ObtenerPorId(int id)
    {
        try
        {
            return Ok(await _servicio.ObtenerPorIdAsync(id));
        }
        catch (MisionNoEncontradaException ex)
        {
            return NoEncontrada(ex);
        }
    }

    /// <summary>POST /api/misiones — 201 con Location, o 400 si rompe una regla.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Mision>> Crear([FromBody] MisionRequest datos)
    {
        try
        {
            var creada = await _servicio.CrearAsync(datos);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
        }
        catch (ValidacionException ex)
        {
            return SolicitudInvalida(ex);
        }
    }

    /// <summary>PUT /api/misiones/{id} — 200 con la misión actualizada, 400 o 404.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Mision), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Mision>> Actualizar(int id, [FromBody] MisionRequest datos)
    {
        try
        {
            return Ok(await _servicio.ActualizarAsync(id, datos));
        }
        catch (ValidacionException ex)
        {
            return SolicitudInvalida(ex);
        }
        catch (MisionNoEncontradaException ex)
        {
            return NoEncontrada(ex);
        }
    }

    /// <summary>DELETE /api/misiones/{id} — 204 sin contenido, o 404.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _servicio.EliminarAsync(id);
            return NoContent();
        }
        catch (MisionNoEncontradaException ex)
        {
            return NoEncontrada(ex);
        }
    }

    private ObjectResult SolicitudInvalida(ValidacionException ex) =>
        Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest, title: "Solicitud inválida");

    private ObjectResult NoEncontrada(MisionNoEncontradaException ex) =>
        Problem(detail: ex.Message, statusCode: StatusCodes.Status404NotFound, title: "Misión no encontrada");
}

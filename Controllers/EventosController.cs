using DSW_3P_PR01.Models;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly IEventoService _service;

    public EventosController(IEventoService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Evento>> Get() => Ok(_service.ObtenerTodos());

    [HttpGet("{id:int}")]
    public ActionResult<Evento> GetById(int id)
    {
        var evento = _service.ObtenerPorId(id);
        return evento is null ? NotFound() : Ok(evento);
    }

    [HttpPost]
    public ActionResult<Evento> Post(Evento evento)
    {
        try
        {
            var creado = _service.Crear(evento);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, Evento evento)
    {
        try
        {
            return _service.Actualizar(id, evento) ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) => _service.Eliminar(id) ? NoContent() : NotFound();
}

using System.Text.Json;
using DSW_3P_PR01.Models;

namespace DSW_3P_PR01.Repositories;

// Primera versión: persistencia en Data/eventos.json
public class EventoJsonRepository : IEventoRepository
{
    private static readonly object _lock = new();
    private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };
    private readonly string _path;

    public EventoJsonRepository(IWebHostEnvironment env)
    {
        var dir = Path.Combine(env.ContentRootPath, "Data");
        Directory.CreateDirectory(dir);
        _path = Path.Combine(dir, "eventos.json");
        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, "[]");
        }
    }

    private List<Evento> Load()
    {
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<Evento>>(json) ?? new List<Evento>();
    }

    private void Save(List<Evento> eventos)
    {
        File.WriteAllText(_path, JsonSerializer.Serialize(eventos, _options));
    }

    public IEnumerable<Evento> GetAll()
    {
        lock (_lock)
        {
            return Load().OrderBy(e => e.Id).ToList();
        }
    }

    public Evento? GetById(int id)
    {
        lock (_lock)
        {
            return Load().FirstOrDefault(e => e.Id == id);
        }
    }

    public Evento Add(Evento evento)
    {
        lock (_lock)
        {
            var eventos = Load();
            evento.Id = eventos.Count == 0 ? 1 : eventos.Max(e => e.Id) + 1;
            eventos.Add(evento);
            Save(eventos);
            return evento;
        }
    }

    public bool Update(Evento evento)
    {
        lock (_lock)
        {
            var eventos = Load();
            var index = eventos.FindIndex(e => e.Id == evento.Id);
            if (index < 0) return false;
            eventos[index] = evento;
            Save(eventos);
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var eventos = Load();
            var removed = eventos.RemoveAll(e => e.Id == id);
            if (removed == 0) return false;
            Save(eventos);
            return true;
        }
    }
}

using System.Text.Json;
using TaskManager.Api.Entities;

namespace TaskManager.Api.Repositories;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonTaskRepository(IConfiguration configuration)
    {
        var dataDirectory = configuration["DataDirectory"] ?? "Data";
        
        if (!Directory.Exists(dataDirectory))
            Directory.CreateDirectory(dataDirectory);
        
        _filePath = Path.Combine(dataDirectory, "tasks.json");
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        
        if (!File.Exists(_filePath))
            File.WriteAllText(_filePath, "[]");
        
        Console.WriteLine($"[JsonTaskRepository] Arquivo JSON: {_filePath}");
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        Console.WriteLine("[GetAllAsync] Lendo arquivo...");
        var json = await File.ReadAllTextAsync(_filePath);
        var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);
        Console.WriteLine($"[GetAllAsync] {tasks?.Count ?? 0} tarefas encontradas");
        return tasks ?? new List<TaskItem>();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        Console.WriteLine($"[GetByIdAsync] Buscando tarefa {id}");
        var tasks = await GetAllAsync();
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        Console.WriteLine("[AddAsync] Iniciando criação de tarefa...");
        
        var tasks = (await GetAllAsync()).ToList();
        
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = null;
        
        Console.WriteLine($"[AddAsync] Nova tarefa ID: {task.Id}");
        
        tasks.Add(task);
        
        var json = JsonSerializer.Serialize(tasks, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
        
        Console.WriteLine("[AddAsync] Tarefa salva com sucesso!");
        
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        Console.WriteLine($"[UpdateAsync] Atualizando tarefa {task.Id}");
        
        var tasks = (await GetAllAsync()).ToList();
        var existingTask = tasks.FirstOrDefault(t => t.Id == task.Id);
        
        if (existingTask == null)
        {
            Console.WriteLine("[UpdateAsync] Tarefa não encontrada");
            return null;
        }
        
        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Status = task.Status;
        existingTask.UpdatedAt = DateTime.UtcNow;
        
        var json = JsonSerializer.Serialize(tasks, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
        
        Console.WriteLine("[UpdateAsync] Tarefa atualizada!");
        
        return existingTask;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Console.WriteLine($"[DeleteAsync] Deletando tarefa {id}");
        
        var tasks = (await GetAllAsync()).ToList();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        
        if (task == null)
        {
            Console.WriteLine("[DeleteAsync] Tarefa não encontrada");
            return false;
        }
        
        tasks.Remove(task);
        
        var json = JsonSerializer.Serialize(tasks, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
        
        Console.WriteLine("[DeleteAsync] Tarefa deletada!");
        
        return true;
    }
}
using System.Text.Json;
using TaskManager.Api.Entities;

namespace TaskManager.Api.Repositories;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
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
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions);
            return tasks ?? new List<TaskItem>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        var tasks = await GetAllAsync();
        return tasks.FirstOrDefault(t => t.Id == id);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await _semaphore.WaitAsync();
        try
        {
            var tasks = (await GetAllAsync()).ToList();
            
            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = null;
            
            tasks.Add(task);
            await SaveTasksAsync(tasks);
            
            return task;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        await _semaphore.WaitAsync();
        try
        {
            var tasks = (await GetAllAsync()).ToList();
            var existingTask = tasks.FirstOrDefault(t => t.Id == task.Id);
            
            if (existingTask == null)
                return null;
            
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.UpdatedAt = DateTime.UtcNow;
            
            await SaveTasksAsync(tasks);
            return existingTask;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var tasks = (await GetAllAsync()).ToList();
            var task = tasks.FirstOrDefault(t => t.Id == id);
            
            if (task == null)
                return false;
            
            tasks.Remove(task);
            await SaveTasksAsync(tasks);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task SaveTasksAsync(List<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
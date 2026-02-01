using TaskManager.Api.DTOs;
using TaskManager.Api.Entities;
using TaskManager.Api.Repositories;

namespace TaskManager.Api.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(
        Entities.TaskStatus? status = null,
        string? orderBy = null,
        string? orderDirection = null)
    {
        var tasks = await _repository.GetAllAsync();
        
        if (status.HasValue)
            tasks = tasks.Where(t => t.Status == status.Value);
        
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            tasks = orderBy.ToLower() switch
            {
                "title" => orderDirection?.ToLower() == "desc" 
                    ? tasks.OrderByDescending(t => t.Title)
                    : tasks.OrderBy(t => t.Title),
                
                "status" => orderDirection?.ToLower() == "desc"
                    ? tasks.OrderByDescending(t => t.Status)
                    : tasks.OrderBy(t => t.Status),
                
                "createdat" => orderDirection?.ToLower() == "desc"
                    ? tasks.OrderByDescending(t => t.CreatedAt)
                    : tasks.OrderBy(t => t.CreatedAt),
                
                _ => tasks.OrderByDescending(t => t.CreatedAt)
            };
        }
        else
        {
            tasks = tasks.OrderByDescending(t => t.CreatedAt);
        }
        
        return tasks.Select(MapToResponseDto);
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        return task != null ? MapToResponseDto(task) : null;
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status
        };
        
        var createdTask = await _repository.AddAsync(task);
        return MapToResponseDto(createdTask);
    }

    public async Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskDto dto)
    {
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            return null;
        
        existingTask.Title = dto.Title;
        existingTask.Description = dto.Description;
        existingTask.Status = dto.Status;
        
        var updatedTask = await _repository.UpdateAsync(existingTask);
        return updatedTask != null ? MapToResponseDto(updatedTask) : null;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static TaskResponseDto MapToResponseDto(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}
using TaskManager.Api.DTOs;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync(
        Entities.TaskStatus? status = null,
        string? orderBy = null,
        string? orderDirection = null,
        DateTime? createdAfter = null,
        DateTime? createdBefore = null);
    
    Task<TaskResponseDto?> GetTaskByIdAsync(Guid id);
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto);
    Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(Guid id);
}
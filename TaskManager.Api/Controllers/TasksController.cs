using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll(
        [FromQuery] Entities.TaskStatus? status = null,
        [FromQuery] string? orderBy = null,
        [FromQuery] string? orderDirection = null,
        [FromQuery] DateTime? createdAfter = null,
        [FromQuery] DateTime? createdBefore = null)
    {
        try
        {
            var tasks = await _taskService.GetAllTasksAsync(status, orderBy, orderDirection, createdAfter, createdBefore);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tarefas");
            return StatusCode(500, new { message = "Erro interno" });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(Guid id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            
            if (task == null)
                return NotFound(new { message = $"Tarefa {id} não encontrada" });
            
            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar tarefa {TaskId}", id);
            return StatusCode(500, new { message = "Erro interno" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create([FromBody] CreateTaskDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var task = await _taskService.CreateTaskAsync(dto);
            
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar tarefa");
            return StatusCode(500, new { message = "Erro interno" });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var updatedTask = await _taskService.UpdateTaskAsync(id, dto);
            
            if (updatedTask == null)
                return NotFound(new { message = $"Tarefa {id} não encontrada" });
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar tarefa {TaskId}", id);
            return StatusCode(500, new { message = "Erro interno" });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            
            if (!deleted)
                return NotFound(new { message = $"Tarefa {id} não encontrada" });
            
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir tarefa {TaskId}", id);
            return StatusCode(500, new { message = "Erro interno" });
        }
    }
}
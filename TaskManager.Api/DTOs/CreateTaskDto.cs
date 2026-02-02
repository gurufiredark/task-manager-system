using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Validation;

namespace TaskManager.Api.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 200 caracteres")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(1000, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 1000 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O status é obrigatório")]
    [ValidEnum(ErrorMessage = "Status inválido. Use: Pending (0), InProgress (1) ou Completed (2)")]
    public Entities.TaskStatus Status { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.DTOs;

public class CreateTaskDto
{
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(1000, MinimumLength = 3)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public Entities.TaskStatus Status { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Validation;

/// <summary>
/// Valida se o valor do enum está definido
/// </summary>
public class ValidEnumAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var enumType = value.GetType();
        
        if (!enumType.IsEnum)
            return new ValidationResult("O valor deve ser um enum válido");

        var isValid = Enum.IsDefined(enumType, value);
        
        if (!isValid)
        {
            var validValues = string.Join(", ", Enum.GetNames(enumType));
            return new ValidationResult(
                $"Status inválido. Valores permitidos: {validValues}"
            );
        }

        return ValidationResult.Success;
    }
}
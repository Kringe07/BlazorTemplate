using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute;

public class RequiredCustomAttribute(string? fieldName = null) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return new ValidationResult($"This {fieldName ?? "value"} is required", validationContext.MemberName != null ? [validationContext.MemberName] : null);

        if (value is Guid guidValue && guidValue == Guid.Empty)
            return new ValidationResult($"This {fieldName ?? "value"} is required", validationContext.MemberName != null ? [validationContext.MemberName] : null);
        
        if (value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue))
            return ValidationResult.Success;
        
        return new ValidationResult($"This {fieldName ?? "value"} is required", validationContext.MemberName != null ? [validationContext.MemberName] : null);
    }
}
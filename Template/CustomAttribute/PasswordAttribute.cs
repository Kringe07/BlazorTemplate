using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute;

//Validate Password It needs at least: [1 capital letter] [1 lowercase letter] [1 number] [1 unique character like : !@#$%^&*] 
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class PasswordAttribute : ValidationAttribute
{
    //Method for attribute validation
    public override bool IsValid(object? value)
    {
        string input = (string?)value ?? string.Empty;
        return input.Any(char.IsUpper) && input.Any(char.IsLower) && input.Any(c => !char.IsLetterOrDigit(c)) && input.Any(char.IsNumber);
    }
}
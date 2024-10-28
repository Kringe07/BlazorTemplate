using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string input = (string?)value ?? string.Empty;
            return input.Any(char.IsUpper) && input.Any(char.IsLower) && input.Any(c => !char.IsLetterOrDigit(c)) && input.Any(char.IsNumber);
        }
    }

}

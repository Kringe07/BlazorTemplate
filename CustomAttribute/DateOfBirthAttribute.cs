using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateOnly input = (DateOnly?)value ?? DateOnly.FromDateTime(DateTime.Now);
            return !input.Equals(DateOnly.FromDateTime(DateTime.Now)) && input.Year != (DateTime.Now.Year - 1);
        }
    }
}

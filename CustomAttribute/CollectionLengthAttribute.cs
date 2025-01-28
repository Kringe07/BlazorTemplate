using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class CollectionLengthAttribute(int minLength, int maxLength) : ValidationAttribute
{
        private int MinLength { get; } = minLength;
        private int MaxLength { get; } = maxLength;

      protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
      {
          if (value is not IEnumerable collection) return ValidationResult.Success;
          var count = collection.Cast<object?>().Count();

           if (count < MinLength)
           {
               return new ValidationResult($"{validationContext.MemberName} must have at least {MinLength} items.", validationContext.MemberName != null ? [validationContext.MemberName] : null);
           }

           if (count > MaxLength)
           {
               return new ValidationResult($"{validationContext.MemberName} must have no more than {MaxLength} items.", validationContext.MemberName != null ? [validationContext.MemberName] : null);
           }

           return ValidationResult.Success;
      }
}
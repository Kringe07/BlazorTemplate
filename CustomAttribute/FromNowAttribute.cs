using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class FromNowAttribute : ValidationAttribute
    {
        private static string GetErrorMessage() => "Date must be past now";

        //Method for attribute validation
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date.Date, DateTime.UtcNow.Date) < 0) 
                return new ValidationResult(GetErrorMessage(),  validationContext.MemberName != null ? [validationContext.MemberName] : null);
            
            return ValidationResult.Success;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class FromNowAttribute : ValidationAttribute
    {
        public FromNowAttribute() { }

        public string GetErrorMessage() => "Date must be past now";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;

            if (DateTime.Compare(date.Date, DateTime.UtcNow.Date) < 0) return new ValidationResult(GetErrorMessage(),  validationContext.MemberName != null ? [validationContext.MemberName] : null);
            else return ValidationResult.Success;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is byte[] fileData)
            {
                if (fileData.Length > _maxFileSize)
                {
                    return new ValidationResult($"File size should not exceed {_maxFileSize / 1024 / 1024} MB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class MaxFileSizeAttribute(long maxFileSize) : ValidationAttribute
    {
        //Validate file on file size 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is byte[] fileData)
            {
                if (fileData.Length > maxFileSize)
                {
                    return new ValidationResult($"File size should not exceed {maxFileSize / 1024 / 1024} MB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}

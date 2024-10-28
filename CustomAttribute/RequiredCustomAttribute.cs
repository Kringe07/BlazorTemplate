using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class RequiredCustomAttribute : RequiredAttribute
    {
        public RequiredCustomAttribute(string? fieldName = null) : base()
        {
            base.ErrorMessage = $"{fieldName} is required.";
        }
    }

}

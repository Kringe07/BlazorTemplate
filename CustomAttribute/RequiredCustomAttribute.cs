using System.ComponentModel.DataAnnotations;

namespace ProjectName.CustomAttribute
{
    public class RequiredCustomAttribute : RequiredAttribute
    {
        // Just says field/object name is required
        public RequiredCustomAttribute(string? fieldName = null) : base()
        {
            base.ErrorMessage = $"{fieldName} is required.";
        }
    }

}

namespace ProjectName.CustomAttribute
{
    public static class StringExtensions
    {
        //Set First letter of sting to Uppercase
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input[0].ToString().ToUpper() + input.Substring(1)
            };

        //Makes a Abbreviation of first 3 char of lastname and first 3 char of firstname
        public static string ToAbbreviation(this string lastName, string firstName)
        {
            if (lastName == null) lastName = string.Empty;
            if (firstName == null) firstName = string.Empty;

            string lastPart = lastName.Length >= 3 ? lastName[..3] : lastName.PadRight(3, ' ');
            string firstPart = firstName.Length >= 3 ? firstName[..3] : firstName.PadRight(3, ' ');

            return (lastPart + firstPart).Replace(" ", "");
        }

        //Makes a Truncate : Trims a string to a specified length, adding "..." if truncated.
        public static string Truncate(this string input, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(input) || maxLength <= 0) return string.Empty;
            return input.Length > maxLength ? input[..maxLength] + suffix : input;
        }

        //Checks if the string contains only numeric characters.
        public static bool IsNumeric(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }

        //Removes all whitespace characters from the string.
        public static string RemoveWhitespace(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        //Replaces part of the string with a mask (e.g., for emails or credit cards).
        public static string Mask(this string input, int visibleStart, int visibleEnd, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(input) || visibleStart + visibleEnd >= input.Length) return input;
            string maskedSection = new string(maskChar, input.Length - visibleStart - visibleEnd);
            return input.Substring(0, visibleStart) + maskedSection + input[^visibleEnd..];
        }

        //Replaces part of the string with a mask for emails
        public static string EmailMask(this string input, int visibleStart, char maskChar = '*')
        {
            if (string.IsNullOrEmpty(input)) return input;

            int atIndex = input.IndexOf('@');
            if (atIndex == -1 || visibleStart >= atIndex)
                return input;

            string visiblePart = input.Substring(0, visibleStart);
            string maskedPart = new string(maskChar, atIndex - visibleStart);
            string domainPart = input[atIndex..];

            return input.Substring(0, visibleStart) + new string(maskChar, atIndex - visibleStart) + input[atIndex..];
        }
    }
}

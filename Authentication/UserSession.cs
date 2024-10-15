namespace ProjectName.Authentication
{
    public class UserSession
    {
        public Guid Id { get; set; } = default;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}

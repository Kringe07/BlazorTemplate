using ProjectName.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectName.DataAccess.Entities;

public abstract class User : AuditableEntity
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string SecretKey { get; set; } = string.Empty;
    [NotMapped]
    public abstract Role Role { get; }
}
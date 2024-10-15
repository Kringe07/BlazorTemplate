using System.ComponentModel.DataAnnotations.Schema;
using ProjectName.DataAccess.Enums;

namespace ProjectName.Entities;

public abstract class User : AuditableEntity
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    [NotMapped]
    public abstract Role Role { get; }
}
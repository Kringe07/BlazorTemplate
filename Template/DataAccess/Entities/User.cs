using ProjectName.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectName.DataAccess.Entities;

public abstract class User : AuditableEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string SecretKey { get; set; } = string.Empty;
    [NotMapped]
    public abstract RoleType RoleType { get; }
}
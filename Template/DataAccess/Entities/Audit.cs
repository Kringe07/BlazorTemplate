using System.ComponentModel.DataAnnotations;

namespace ProjectName.DataAccess.Entities;

public class Audit
{
    public int Id { get; set; }
    public required Guid EntityId { get; set; }
    public required DateTimeOffset TimeStamp { get; set; }
    [MaxLength(50)]
    public required string TableName { get; set; }
    [MaxLength(50)]
    public required string Action { get; set; }
    [MaxLength(1000)]
    public required string Changes { get; set; }
    [MaxLength(1000)]
    public required string OriginalValue { get; set; }
    public Guid? UserId { get; set; }
    [MaxLength(50)]
    public string? UserEmail { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectName.DataAccess.Entities;
using System.Text.Json;
using ProjectName.CustomAttribute;
using ProjectName.Models;

namespace ProjectName.DataAccess.DatabaseContext;

public class ProjectNameContext(DbContextOptions<ProjectNameContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Audit> Audits { get; set; } = null!;

    public async Task<int> SaveChangesAndAudit(AuditUser user)
    {
        CreateAudits(user);
        return await SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().UseTpcMappingStrategy();
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<Customer>().ToTable("Customers");
        base.OnModelCreating(modelBuilder);
    }

    private void CreateAudits(AuditUser user)
    {
        var auditEntries = new List<Audit>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Added or EntityState.Modified or EntityState.Deleted && entry.Entity is AuditableEntity auditableEntity)
            {
                auditEntries.Add(new Audit
                {
                    EntityId = auditableEntity.Id,
                    TableName = entry.Entity.GetType().Name,
                    Action = entry.State.ToString(),
                    TimeStamp = DateTime.UtcNow,
                    Changes = GetChanges(entry),
                    UserId = user.Id,
                    UserEmail = user.Name,
                    OriginalValue = GetOriginalValue(entry)
                });
            }
        }

        Audits.AddRange(auditEntries);
    }

    private static string GetChanges(EntityEntry entry) =>
        JsonSerializer.Serialize(entry.Properties.Where(p => p.IsModified && !CheckForSensitiveProperty(p)).ToDictionary(p => p.Metadata.Name,
            p => (entry.State == EntityState.Deleted ? p.OriginalValue : p.CurrentValue)!));

    private static string GetOriginalValue(EntityEntry entry) =>
        JsonSerializer.Serialize(entry.Properties.Where(p => !CheckForSensitiveProperty(p)).ToDictionary(p => p.Metadata.Name, ProcessPropertyEntry));

    private static bool CheckForSensitiveProperty(PropertyEntry propertyEntry) =>
        propertyEntry.Metadata.PropertyInfo!.GetCustomAttributes(typeof(SensitiveData), false).Any();

    private static string ProcessPropertyEntry(PropertyEntry propertyInfo) =>
        (propertyInfo.OriginalValue is DateTime dateTime ? dateTime.ToString("yyyy-MM-dd HH:mm") : propertyInfo.OriginalValue?.ToString())!;
}
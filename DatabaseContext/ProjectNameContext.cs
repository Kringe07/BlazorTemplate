using Microsoft.EntityFrameworkCore;
using ProjectName.Entities;

namespace ProjectName.DataAccess.DatabaseContext;

public class ProjectNameContext(DbContextOptions<ProjectNameContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().UseTpcMappingStrategy();
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<Customer>().ToTable("Customers");
        base.OnModelCreating(modelBuilder);
    }
}
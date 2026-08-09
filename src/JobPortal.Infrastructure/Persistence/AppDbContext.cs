using Microsoft.EntityFrameworkCore;
using JobPortal.Domain.Entities;

namespace JobPortal.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(user => user.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(user => user.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Student>()
            .Property(student => student.Status)
            .HasConversion<string>();
    }
}
using Microsoft.EntityFrameworkCore;
using JobPortal.Domain.Entities;

namespace JobPortal.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(user => user.Email)
            .HasMaxLength(320);

        modelBuilder.Entity<User>()
            .Property(user => user.FullName)
            .HasMaxLength(200);

        modelBuilder.Entity<User>()
            .Property(user => user.AccountType)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<User>()
            .Property(user => user.Status)
            .HasConversion<string>();
    }
}

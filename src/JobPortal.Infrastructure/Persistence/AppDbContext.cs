using Microsoft.EntityFrameworkCore;
using JobPortal.Domain.Entities;

namespace JobPortal.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private static readonly Guid EngineeringCategoryId = Guid.Parse(
        "11111111-1111-1111-1111-111111111111"
    );
    private static readonly Guid DesignCategoryId = Guid.Parse(
        "22222222-2222-2222-2222-222222222222"
    );
    private static readonly Guid MarketingCategoryId = Guid.Parse(
        "33333333-3333-3333-3333-333333333333"
    );
    private static readonly Guid SalesCategoryId = Guid.Parse(
        "44444444-4444-4444-4444-444444444444"
    );
    private static readonly Guid FinanceCategoryId = Guid.Parse(
        "55555555-5555-5555-5555-555555555555"
    );
    private static readonly Guid HumanResourcesCategoryId = Guid.Parse(
        "66666666-6666-6666-6666-666666666666"
    );
    private static readonly Guid OperationsCategoryId = Guid.Parse(
        "77777777-7777-7777-7777-777777777777"
    );
    private static readonly Guid CustomerSupportCategoryId = Guid.Parse(
        "88888888-8888-8888-8888-888888888888"
    );

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<Category> Categories => Set<Category>();

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

        modelBuilder.Entity<Job>()
            .Property(job => job.Title)
            .HasMaxLength(200);

        modelBuilder.Entity<Job>()
            .Property(job => job.Description)
            .HasMaxLength(10000);

        modelBuilder.Entity<Job>()
            .Property(job => job.Requirements)
            .HasMaxLength(10000);

        modelBuilder.Entity<Job>()
            .Property(job => job.Location)
            .HasMaxLength(300);

        modelBuilder.Entity<Job>()
            .Property(job => job.EmploymentType)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Job>()
            .Property(job => job.ExperienceLevel)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Job>()
            .Property(job => job.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<Job>()
            .Property(job => job.Salary)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Job>()
            .HasOne(job => job.Employer)
            .WithMany(user => user.Jobs)
            .HasForeignKey(job => job.EmployerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Job>()
            .HasOne(job => job.Category)
            .WithMany(category => category.Jobs)
            .HasForeignKey(job => job.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasIndex(job => new { job.EmployerId, job.CreatedAtUtc });

        modelBuilder.Entity<Category>()
            .Property(category => category.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = EngineeringCategoryId, Name = "Engineering" },
            new Category { Id = DesignCategoryId, Name = "Design" },
            new Category { Id = MarketingCategoryId, Name = "Marketing" },
            new Category { Id = SalesCategoryId, Name = "Sales" },
            new Category { Id = FinanceCategoryId, Name = "Finance" },
            new Category
            {
                Id = HumanResourcesCategoryId,
                Name = "Human Resources"
            },
            new Category { Id = OperationsCategoryId, Name = "Operations" },
            new Category
            {
                Id = CustomerSupportCategoryId,
                Name = "Customer Support"
            }
        );
    }
}

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

    public DbSet<Job> Jobs => Set<Job>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<JobApplication> Applications => Set<JobApplication>();

    public DbSet<JobSeekerProfile> JobSeekerProfiles => Set<JobSeekerProfile>();

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

        modelBuilder.Entity<JobApplication>()
            .HasKey(application => new
            {
                application.JobId,
                application.ApplicantId
            });

        modelBuilder.Entity<JobApplication>()
            .Property(application => application.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        modelBuilder.Entity<JobApplication>()
            .HasOne(application => application.Job)
            .WithMany(job => job.Applications)
            .HasForeignKey(application => application.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(application => application.Applicant)
            .WithMany(user => user.Applications)
            .HasForeignKey(application => application.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasIndex(application => application.ApplicantId);

        modelBuilder.Entity<JobSeekerProfile>()
            .HasKey(profile => profile.UserId);

        modelBuilder.Entity<JobSeekerProfile>()
            .Property(profile => profile.Skills)
            .HasMaxLength(2000);

        modelBuilder.Entity<JobSeekerProfile>()
            .Property(profile => profile.Education)
            .HasMaxLength(5000);

        modelBuilder.Entity<JobSeekerProfile>()
            .Property(profile => profile.Experience)
            .HasMaxLength(10000);

        modelBuilder.Entity<JobSeekerProfile>()
            .Property(profile => profile.Headline)
            .HasMaxLength(200);

        modelBuilder.Entity<JobSeekerProfile>()
            .Property(profile => profile.CvUrl)
            .HasMaxLength(2048);

        modelBuilder.Entity<JobSeekerProfile>()
            .HasOne(profile => profile.User)
            .WithOne(user => user.JobSeekerProfile)
            .HasForeignKey<JobSeekerProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .Property(category => category.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        modelBuilder.Entity<Category>()
            .Property(category => category.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using JobPortal.Application.Common.Interfaces;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;

namespace JobPortal.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordService _passwordService;
    private readonly IConfiguration _configuration;

    public DatabaseSeeder(
        AppDbContext dbContext,
        IPasswordService passwordService,
        IConfiguration configuration
    )
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
        _configuration = configuration;
    }

    public async Task SeedDefaultAdminAsync()
    {
        var email = _configuration["DefaultAdmin:Email"];
        var password = _configuration["DefaultAdmin:Password"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        email = email.Trim().ToLowerInvariant();

        var adminExists = await _dbContext.Users
            .AnyAsync(user =>
                user.Email == email &&
                user.Role == UserRole.Admin
            );

        if (adminExists)
        {
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "System",
            LastName = "Admin",
            Email = email,
            PasswordHash = _passwordService.HashPassword(password),
            Role = UserRole.Admin,
            Status = UserStatus.MustChangePassword,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Users.Add(admin);

        await _dbContext.SaveChangesAsync();
    }
}
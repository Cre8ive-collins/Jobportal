using JobPortal.Application.Common.Interfaces;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public UserService(
        IUserRepository userRepository,
        IPasswordService passwordService
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _userRepository.EmailExistsAsync(email);

        if (emailExists)
        {
            throw new InvalidOperationException(
                "A user with this email already exists."
            );
        }

        var temporaryPassword =
            _passwordService.GenerateTemporaryPassword();

        var passwordHash =
            _passwordService.HashPassword(temporaryPassword);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash = passwordHash,
            Role = request.Role,
            Status = UserStatus.MustChangePassword,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return MapToResponse(user);
    }

    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        return MapToResponse(user);
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }
}
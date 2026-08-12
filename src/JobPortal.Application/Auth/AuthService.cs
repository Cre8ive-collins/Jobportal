using JobPortal.Application.Common.Exceptions;
using JobPortal.Application.Common.Interfaces;
using JobPortal.Application.Users;
using JobPortal.Domain.Entities;
using JobPortal.Domain.Enums;

namespace JobPortal.Application.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ITokenService tokenService
    )
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request)
    {
        var email = NormalizeEmail(request.Email);

        if (await _userRepository.EmailExistsAsync(email))
        {
            throw new ConflictException(
                "An account with this email already exists."
            );
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = _passwordService.HashPassword(request.Password),
            AccountType = request.AccountType.Trim(),
            Status = UserStatus.Active,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        return MapToResponse(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(
            NormalizeEmail(request.Email)
        );

        if (user is null ||
            !_passwordService.VerifyPassword(
                request.Password,
                user.PasswordHash
            ))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (user.Status != UserStatus.Active)
        {
            throw new UnauthorizedException("This account is not active.");
        }

        return new LoginResponse
        {
            User = MapToResponse(user),
            Token = _tokenService.CreateToken(user)
        };
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            AccountType = user.AccountType,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }
}

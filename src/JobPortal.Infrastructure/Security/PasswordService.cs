using System.Security.Cryptography;
using JobPortal.Application.Common.Interfaces;

namespace JobPortal.Infrastructure.Security;

public class PasswordService : IPasswordService
{
    public string GenerateTemporaryPassword()
    {
        const string uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string lowercase = "abcdefghijkmnopqrstuvwxyz";
        const string numbers = "23456789";
        const string symbols = "!@#$%";

        var characters = uppercase + lowercase + numbers + symbols;

        var password = new char[12];

        password[0] = uppercase[RandomNumberGenerator.GetInt32(uppercase.Length)];
        password[1] = lowercase[RandomNumberGenerator.GetInt32(lowercase.Length)];
        password[2] = numbers[RandomNumberGenerator.GetInt32(numbers.Length)];
        password[3] = symbols[RandomNumberGenerator.GetInt32(symbols.Length)];

        for (var index = 4; index < password.Length; index++)
        {
            password[index] = characters[
                RandomNumberGenerator.GetInt32(characters.Length)
            ];
        }

        RandomNumberGenerator.Shuffle(password);

        return new string(password);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
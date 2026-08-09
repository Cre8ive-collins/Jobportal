namespace JobPortal.Application.Common.Interfaces;

public interface IPasswordService
{
    string GenerateTemporaryPassword();

    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
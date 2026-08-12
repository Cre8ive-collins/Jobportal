using JobPortal.Domain.Entities;

namespace JobPortal.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}

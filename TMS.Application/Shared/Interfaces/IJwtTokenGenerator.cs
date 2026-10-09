namespace TMS.Application.Shared.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, string role);
}

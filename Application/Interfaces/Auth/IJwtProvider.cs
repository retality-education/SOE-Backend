using Application.Services;
using Core.Models;
using System.Security.Claims;

namespace Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        RefreshToken GenerateRefreshToken(User user);
        ClaimsPrincipal? ValidateToken(string token, bool isRefreshToken = false);
        Guid? GetRefreshTokenId(string refreshToken);
        Guid? GetUserIdFromAccessTokem(string accessToken);
    }
}
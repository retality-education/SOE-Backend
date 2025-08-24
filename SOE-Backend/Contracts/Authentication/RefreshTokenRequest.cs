using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Authethication
{
    public record RefreshTokenRequest(
            string RefreshToken
            );
}

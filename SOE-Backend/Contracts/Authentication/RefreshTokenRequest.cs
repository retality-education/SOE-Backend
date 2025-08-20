using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Authethication
{
    public record RefreshTokenRequest(
            [Required] string RefreshToken
            );
}

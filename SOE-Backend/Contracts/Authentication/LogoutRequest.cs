using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Authentication
{
    public record LogoutRequest(
            [Required] string RefreshToken
            );
}

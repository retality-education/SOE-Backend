using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Authentication
{
    public record LogoutRequest(
            string RefreshToken
            );
}

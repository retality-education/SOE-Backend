using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Authentication
{
    internal record LogoutRequest(
            string RefreshToken
            );
}

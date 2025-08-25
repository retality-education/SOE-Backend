using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Authethication
{
    internal record RefreshTokenRequest(
            string RefreshToken
            );
}

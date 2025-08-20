using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record ForgotPasswordRequest
    (
        [Required] string Email
    );
}

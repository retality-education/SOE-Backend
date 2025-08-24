using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    public record ForgotPasswordRequest
    (
        [Required] string Email
    );
}

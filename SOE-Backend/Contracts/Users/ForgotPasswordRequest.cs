using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    internal record ForgotPasswordRequest
    (
        [Required] string Email
    );
}

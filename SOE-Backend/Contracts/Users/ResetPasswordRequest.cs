using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record ResetPasswordRequest
    (
        [Required] string Email,
        [Required] string ResetCode,
        [Required] string NewPassword
    );
}

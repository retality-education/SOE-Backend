using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    internal record ResetPasswordRequest
    (
        [Required] string Email,
        [Required] string ResetCode,
        [Required] string NewPassword
    );
}

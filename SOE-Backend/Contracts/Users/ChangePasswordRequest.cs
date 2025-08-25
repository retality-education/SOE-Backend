using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    internal record ChangePasswordRequest(
        [Required] string CurrentPassword,
        [Required]string NewPassword
    );
}

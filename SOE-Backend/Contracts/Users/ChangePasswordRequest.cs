using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    public record ChangePasswordRequest(
        [Required] string CurrentPassword,
        [Required]string NewPassword
    );
}

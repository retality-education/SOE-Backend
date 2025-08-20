using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record ChangePasswordRequest(
        [Required] string CurrentPassword,
        [Required]string NewPassword
    );
}

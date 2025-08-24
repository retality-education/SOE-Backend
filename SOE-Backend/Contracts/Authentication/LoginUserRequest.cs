using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password
        );
}

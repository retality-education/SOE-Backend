using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record LoginUserRequest(
        [Required] string Email,
        [Required] string Password
        );
}

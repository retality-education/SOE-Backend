using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record RegisterUserRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email);
}
using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    internal record LoginUserRequest(
        [Required] string Email,
        [Required] string Password
        );
}

using System.ComponentModel.DataAnnotations;

namespace SOE_Backend.Contracts.Users
{
    public record ChangeAvatarRequest([Required]IFormFile Avatar);
}

using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    public record ChangeAvatarRequest([Required]IFormFile Avatar);
}

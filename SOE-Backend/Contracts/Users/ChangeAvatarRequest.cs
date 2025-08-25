using System.ComponentModel.DataAnnotations;

namespace SOEBackend.Contracts.Users
{
    internal record ChangeAvatarRequest([Required]IFormFile Avatar);
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class UserRoleEntity
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}

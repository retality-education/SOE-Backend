using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class UserUnlockedEndingEntity
{
    public Guid UserId { get; set; }

    public int EndingId { get; set; }

    public DateTime? UnlockedAt { get; set; }

    public virtual PartEndingEntity Ending { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class PartEndingEntity
{
    public int EndingId { get; set; }

    public Guid PartId { get; set; }

    public string EndingName { get; set; } = null!;

    public string? EndingDescription { get; set; }

    public int EndingOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual BookPartEntity Part { get; set; } = null!;

    public virtual ICollection<UserUnlockedEndingEntity> UserUnlockedEndings { get; set; } = new List<UserUnlockedEndingEntity>();
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class NotificationEntity
{
    public Guid NotificationId { get; set; }

    public Guid? UserId { get; set; }

    public string NotificationText { get; set; } = null!;

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? EntityTypeId { get; set; }

    public Guid? EntityId { get; set; }

    public virtual EntityTypeEntity? EntityType { get; set; }

    public virtual UserEntity? User { get; set; }
}

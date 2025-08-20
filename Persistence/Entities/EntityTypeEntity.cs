using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class EntityTypeEntity
{
    public int EntityTypeId { get; set; }

    public string TableName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class IssueStatusEntity
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<IssueEntity> Issues { get; set; } = new List<IssueEntity>();
}

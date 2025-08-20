using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class IssueResponseEntity
{
    public Guid ResponseId { get; set; }

    public Guid? IssueId { get; set; }

    public Guid? UserId { get; set; }

    public string ResponseText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual IssueEntity? Issue { get; set; }

    public virtual UserEntity? User { get; set; }
}

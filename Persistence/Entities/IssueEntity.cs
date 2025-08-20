using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class IssueEntity
{
    public Guid IssueId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? BookId { get; set; }

    public string IssueText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? StatusId { get; set; }

    public virtual BookEntity? Book { get; set; }

    public virtual ICollection<IssueResponseEntity> IssueResponses { get; set; } = new List<IssueResponseEntity>();

    public virtual IssueStatusEntity? Status { get; set; }

    public virtual UserEntity? User { get; set; }
}

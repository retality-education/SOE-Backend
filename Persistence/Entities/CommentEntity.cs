using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class CommentEntity
{
    public Guid CommentId { get; set; }

    public Guid? BookId { get; set; }

    public Guid? UserId { get; set; }

    public string CommentText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual BookEntity? Book { get; set; }

    public virtual ICollection<CommentLikeEntity> CommentLikes { get; set; } = new List<CommentLikeEntity>();

    public virtual UserEntity? User { get; set; }
}

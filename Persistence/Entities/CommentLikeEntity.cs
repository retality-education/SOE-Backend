using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class CommentLikeEntity
{
    public Guid LikeId { get; set; }

    public Guid? CommentId { get; set; }

    public Guid? UserId { get; set; }

    public virtual CommentEntity? Comment { get; set; }

    public virtual UserEntity? User { get; set; }
}

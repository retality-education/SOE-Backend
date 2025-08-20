using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookmarkEntity
{
    public Guid BookId { get; set; }

    public Guid UserId { get; set; }

    public int BookmarkTypeId { get; set; }

    public virtual BookEntity Book { get; set; } = null!;

    public virtual BookmarkTypeEntity BookmarkType { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}

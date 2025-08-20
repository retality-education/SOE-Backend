using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookmarkTypeEntity
{
    public int BookmarkTypeId { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<BookmarkEntity> Bookmarks { get; set; } = new List<BookmarkEntity>();
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class TagEntity
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
}

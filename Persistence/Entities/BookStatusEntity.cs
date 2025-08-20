using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookStatusEntity
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
}

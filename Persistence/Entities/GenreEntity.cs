using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class GenreEntity
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
}

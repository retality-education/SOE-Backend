using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookStatisticEntity
{
    public Guid BookId { get; set; }

    public decimal? Rating { get; set; }

    public int? RatingCount { get; set; }

    public int? ReadCount { get; set; }

    public int? FavoriteCount { get; set; }

    public virtual BookEntity Book { get; set; } = null!;
}

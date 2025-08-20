using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class UserStatisticEntity
{
    public Guid UserId { get; set; }

    public int? BooksRead { get; set; }

    public int? AverageReadingTime { get; set; }

    public DateTime? LastActive { get; set; }

    public virtual UserEntity User { get; set; } = null!;
}

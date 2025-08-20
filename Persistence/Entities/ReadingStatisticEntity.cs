using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class ReadingStatisticEntity
{
    public Guid ReadingStatId { get; set; }

    public Guid? BookId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? LastReadTime { get; set; }

    public decimal? Progress { get; set; }

    public int? Rating { get; set; }

    public int? TimeSpent { get; set; }

    public virtual BookEntity? Book { get; set; }

    public virtual UserEntity? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookPartEntity
{
    public Guid PartId { get; set; }

    public Guid? BookId { get; set; }

    public string PartName { get; set; } = null!;

    public string ContentPath { get; set; } = null!;

    public int PartOrder { get; set; }

    public int? CountOfEndings { get; set; }

    public virtual BookEntity? Book { get; set; }

    public virtual ICollection<PartEndingEntity> PartEndings { get; set; } = new List<PartEndingEntity>();
}

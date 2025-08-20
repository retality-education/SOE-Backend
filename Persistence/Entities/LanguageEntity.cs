using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class LanguageEntity
{
    public int LanguageId { get; set; }

    public string LanguageName { get; set; } = null!;

    public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
}

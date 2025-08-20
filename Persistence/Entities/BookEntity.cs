using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class BookEntity
{
    public Guid BookId { get; set; }

    public string BookName { get; set; } = null!;

    public string? Annotation { get; set; }

    public Guid AuthorId { get; set; }

    public string? CoverId { get; set; }

    public int StatusId { get; set; }

    public int LanguageId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual UserEntity Author { get; set; } = null!;

    public virtual ICollection<BookPartEntity> BookParts { get; set; } = new List<BookPartEntity>();

    public virtual BookStatisticEntity? BookStatistic { get; set; }

    public virtual ICollection<BookmarkEntity> Bookmarks { get; set; } = new List<BookmarkEntity>();

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

    public virtual ICollection<IssueEntity> Issues { get; set; } = new List<IssueEntity>();

    public virtual LanguageEntity Language { get; set; } = null!;

    public virtual ICollection<ReadingStatisticEntity> ReadingStatistics { get; set; } = new List<ReadingStatisticEntity>();

    public virtual BookStatusEntity Status { get; set; } = null!;

    public virtual ICollection<GenreEntity> Genres { get; set; } = new List<GenreEntity>();

    public virtual ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}

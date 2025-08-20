using System;
using System.Collections.Generic;

namespace Persistence.Entities;

public partial class UserEntity
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? AvatarId { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<BookmarkEntity> Bookmarks { get; set; } = new List<BookmarkEntity>();

    public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();

    public virtual ICollection<CommentLikeEntity> CommentLikes { get; set; } = new List<CommentLikeEntity>();

    public virtual ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

    public virtual ICollection<IssueResponseEntity> IssueResponses { get; set; } = new List<IssueResponseEntity>();

    public virtual ICollection<IssueEntity> Issues { get; set; } = new List<IssueEntity>();

    public virtual ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();

    public virtual ICollection<ReadingStatisticEntity> ReadingStatistics { get; set; } = new List<ReadingStatisticEntity>();

    public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();

    public virtual UserRoleEntity Role { get; set; } = null!;

    public virtual UserStatisticEntity? UserStatistic { get; set; }

    public virtual ICollection<UserUnlockedEndingEntity> UserUnlockedEndings { get; set; } = new List<UserUnlockedEndingEntity>();

    public virtual ICollection<BookEntity> BooksNavigation { get; set; } = new List<BookEntity>();
}

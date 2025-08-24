using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public partial class SoeBackendContext : DbContext
{
    public SoeBackendContext()
    {
    }

    public SoeBackendContext(DbContextOptions<SoeBackendContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookEntity> Books { get; set; }

    public virtual DbSet<BookPartEntity> BookParts { get; set; }

    public virtual DbSet<BookStatisticEntity> BookStatistics { get; set; }

    public virtual DbSet<BookStatusEntity> BookStatuses { get; set; }

    public virtual DbSet<BookmarkEntity> Bookmarks { get; set; }

    public virtual DbSet<BookmarkTypeEntity> BookmarkTypes { get; set; }

    public virtual DbSet<CommentEntity> Comments { get; set; }

    public virtual DbSet<CommentLikeEntity> CommentLikes { get; set; }

    public virtual DbSet<EntityTypeEntity> EntityTypes { get; set; }

    public virtual DbSet<GenreEntity> Genres { get; set; }

    public virtual DbSet<IssueEntity> Issues { get; set; }

    public virtual DbSet<IssueResponseEntity> IssueResponses { get; set; }

    public virtual DbSet<IssueStatusEntity> IssueStatuses { get; set; }

    public virtual DbSet<LanguageEntity> Languages { get; set; }

    public virtual DbSet<NotificationEntity> Notifications { get; set; }

    public virtual DbSet<PartEndingEntity> PartEndings { get; set; }

    public virtual DbSet<ReadingStatisticEntity> ReadingStatistics { get; set; }

    public virtual DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    public virtual DbSet<TagEntity> Tags { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    public virtual DbSet<UserRoleEntity> UserRoles { get; set; }

    public virtual DbSet<UserStatisticEntity> UserStatistics { get; set; }

    public virtual DbSet<UserUnlockedEndingEntity> UserUnlockedEndings { get; set; }

    public virtual DbSet<SortOptionEntity> SortOptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=SOE-Backend;Username=postgres;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("books_pkey");

            entity.ToTable("books");

            entity.Property(e => e.BookId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("book_id");
            entity.Property(e => e.Annotation).HasColumnName("annotation");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookName)
                .HasMaxLength(100)
                .HasColumnName("book_name");
            entity.Property(e => e.CoverId)
                .HasMaxLength(100)
                .HasColumnName("cover_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Description)
            .HasColumnName("description")
            .HasDefaultValue("");
            entity.Property(e => e.IsAdultContent)
                .HasColumnName("is_adult_content")
                .HasDefaultValue(false);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("books_author_id_fkey");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_language_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Books)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_status_id_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookGenre",
                    r => r.HasOne<GenreEntity>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("book_genres_genre_id_fkey"),
                    l => l.HasOne<BookEntity>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("book_genres_book_id_fkey"),
                    j =>
                    {
                        j.HasKey("BookId", "GenreId").HasName("book_genres_pkey");
                        j.ToTable("book_genres");
                        j.IndexerProperty<Guid>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");
                    });

            entity.HasMany(d => d.Tags).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookTag",
                    r => r.HasOne<TagEntity>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("book_tags_tag_id_fkey"),
                    l => l.HasOne<BookEntity>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("book_tags_book_id_fkey"),
                    j =>
                    {
                        j.HasKey("BookId", "TagId").HasName("book_tags_pkey");
                        j.ToTable("book_tags");
                        j.IndexerProperty<Guid>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.BooksNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "Favorite",
                    r => r.HasOne<UserEntity>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("favorites_user_id_fkey"),
                    l => l.HasOne<BookEntity>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("favorites_book_id_fkey"),
                    j =>
                    {
                        j.HasKey("BookId", "UserId").HasName("favorites_pkey");
                        j.ToTable("favorites");
                        j.IndexerProperty<Guid>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<SortOptionEntity>(entity =>
        {
            entity.HasKey(e => e.SortOptionId)
                .HasName("sort_options_pkey");

            entity.ToTable("sort_options");

            entity.Property(e => e.SortOptionId)
                .HasColumnName("sort_option_id")
                .UseIdentityColumn();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("display_name");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");

            entity.Property(e => e.Order)
                .HasColumnName("order")
                .HasDefaultValue(0);

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Name, "sort_options_name_key")
                .IsUnique();
        });

        modelBuilder.Entity<BookPartEntity>(entity =>
        {
            entity.HasKey(e => e.PartId).HasName("book_parts_pkey");

            entity.ToTable("book_parts");

            entity.HasIndex(e => e.ContentPath, "book_parts_content_path_key").IsUnique();

            entity.Property(e => e.PartId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("part_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ContentPath)
                .HasMaxLength(200)
                .HasColumnName("content_path");
            entity.Property(e => e.CountOfEndings).HasColumnName("count_of_endings");
            entity.Property(e => e.PartName)
                .HasMaxLength(100)
                .HasColumnName("part_name");
            entity.Property(e => e.PartOrder).HasColumnName("part_order");

            entity.HasOne(d => d.Book).WithMany(p => p.BookParts)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("book_parts_book_id_fkey");
        });

        modelBuilder.Entity<BookStatisticEntity>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("book_statistics_pkey");

            entity.ToTable("book_statistics");

            entity.Property(e => e.BookId)
                .ValueGeneratedNever()
                .HasColumnName("book_id");
            entity.Property(e => e.FavoriteCount)
                .HasDefaultValue(0)
                .HasColumnName("favorite_count");
            entity.Property(e => e.Rating)
                .HasPrecision(3, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("rating");
            entity.Property(e => e.RatingCount)
                .HasDefaultValue(0)
                .HasColumnName("rating_count");
            entity.Property(e => e.ReadCount)
                .HasDefaultValue(0)
                .HasColumnName("read_count");

            entity.HasOne(d => d.Book).WithOne(p => p.BookStatistic)
                .HasForeignKey<BookStatisticEntity>(d => d.BookId)
                .HasConstraintName("book_statistics_book_id_fkey");
        });

        modelBuilder.Entity<BookStatusEntity>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("book_statuses_pkey");

            entity.ToTable("book_statuses");

            entity.HasIndex(e => e.StatusName, "book_statuses_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<BookmarkEntity>(entity =>
        {
            entity.HasKey(e => new { e.BookId, e.UserId, e.BookmarkTypeId }).HasName("bookmarks_pkey");

            entity.ToTable("bookmarks");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.BookmarkTypeId).HasColumnName("bookmark_type_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("bookmarks_book_id_fkey");

            entity.HasOne(d => d.BookmarkType).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.BookmarkTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bookmarks_bookmark_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Bookmarks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("bookmarks_user_id_fkey");
        });

        modelBuilder.Entity<BookmarkTypeEntity>(entity =>
        {
            entity.HasKey(e => e.BookmarkTypeId).HasName("bookmark_types_pkey");

            entity.ToTable("bookmark_types");

            entity.HasIndex(e => e.DisplayName, "bookmark_types_display_name_key").IsUnique();

            entity.Property(e => e.BookmarkTypeId).HasColumnName("bookmark_type_id");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(50)
                .HasColumnName("display_name");
        });

        modelBuilder.Entity<CommentEntity>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("comment_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CommentText).HasColumnName("comment_text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_book_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_user_id_fkey");
        });

        modelBuilder.Entity<CommentLikeEntity>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("comment_likes_pkey");

            entity.ToTable("comment_likes");

            entity.HasIndex(e => new { e.CommentId, e.UserId }, "comment_likes_comment_id_user_id_key").IsUnique();

            entity.Property(e => e.LikeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("like_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Comment).WithMany(p => p.CommentLikes)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comment_likes_comment_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.CommentLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comment_likes_user_id_fkey");
        });

        modelBuilder.Entity<EntityTypeEntity>(entity =>
        {
            entity.HasKey(e => e.EntityTypeId).HasName("entity_types_pkey");

            entity.ToTable("entity_types");

            entity.HasIndex(e => e.TableName, "entity_types_table_name_key").IsUnique();

            entity.Property(e => e.EntityTypeId).HasColumnName("entity_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("table_name");
        });

        modelBuilder.Entity<GenreEntity>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.GenreName, "genres_genre_name_key").IsUnique();

            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.GenreName)
                .HasMaxLength(100)
                .HasColumnName("genre_name");
        });

        modelBuilder.Entity<IssueEntity>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("issues_pkey");

            entity.ToTable("issues");

            entity.Property(e => e.IssueId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("issue_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IssueText).HasColumnName("issue_text");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(1)
                .HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Issues)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("issues_book_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Issues)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("issues_status_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Issues)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("issues_user_id_fkey");
        });

        modelBuilder.Entity<IssueResponseEntity>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("issue_responses_pkey");

            entity.ToTable("issue_responses");

            entity.Property(e => e.ResponseId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("response_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IssueId).HasColumnName("issue_id");
            entity.Property(e => e.ResponseText).HasColumnName("response_text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Issue).WithMany(p => p.IssueResponses)
                .HasForeignKey(d => d.IssueId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("issue_responses_issue_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.IssueResponses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("issue_responses_user_id_fkey");
        });

        modelBuilder.Entity<IssueStatusEntity>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("issue_statuses_pkey");

            entity.ToTable("issue_statuses");

            entity.HasIndex(e => e.StatusName, "issue_statuses_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<LanguageEntity>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("languages_pkey");

            entity.ToTable("languages");

            entity.HasIndex(e => e.LanguageName, "languages_language_name_key").IsUnique();

            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(100)
                .HasDefaultValueSql("'Russian'::character varying")
                .HasColumnName("language_name");
        });

        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("notification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EntityId).HasColumnName("entity_id");
            entity.Property(e => e.EntityTypeId).HasColumnName("entity_type_id");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.NotificationText).HasColumnName("notification_text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.EntityType).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.EntityTypeId)
                .HasConstraintName("notifications_entity_type_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("notifications_user_id_fkey");
        });

        modelBuilder.Entity<PartEndingEntity>(entity =>
        {
            entity.HasKey(e => e.EndingId).HasName("part_endings_pkey");

            entity.ToTable("part_endings");

            entity.HasIndex(e => e.PartId, "idx_part_endings_part");

            entity.Property(e => e.EndingId).HasColumnName("ending_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.EndingDescription).HasColumnName("ending_description");
            entity.Property(e => e.EndingName)
                .HasMaxLength(100)
                .HasColumnName("ending_name");
            entity.Property(e => e.EndingOrder).HasColumnName("ending_order");
            entity.Property(e => e.PartId).HasColumnName("part_id");

            entity.HasOne(d => d.Part).WithMany(p => p.PartEndings)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("part_endings_part_id_fkey");
        });

        modelBuilder.Entity<ReadingStatisticEntity>(entity =>
        {
            entity.HasKey(e => e.ReadingStatId).HasName("reading_statistics_pkey");

            entity.ToTable("reading_statistics");

            entity.Property(e => e.ReadingStatId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("reading_stat_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.LastReadTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_read_time");
            entity.Property(e => e.Progress)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("progress");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");
            entity.Property(e => e.TimeSpent)
                .HasDefaultValue(0)
                .HasColumnName("time_spent");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Book).WithMany(p => p.ReadingStatistics)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("reading_statistics_book_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.ReadingStatistics)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("reading_statistics_user_id_fkey");
        });

        modelBuilder.Entity<RefreshTokenEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.HasIndex(e => new { e.UserId, e.Token }, "idx_refresh_tokens_active").HasFilter("(is_revoked = false)");

            entity.HasIndex(e => e.Token, "idx_refresh_tokens_token");

            entity.HasIndex(e => e.UserId, "idx_refresh_tokens_user_id");

            entity.HasIndex(e => e.Token, "refresh_tokens_token_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked)
                .HasDefaultValue(false)
                .HasColumnName("is_revoked");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("refresh_tokens_user_id_fkey");
        });

        modelBuilder.Entity<TagEntity>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("tags_pkey");

            entity.ToTable("tags");

            entity.HasIndex(e => e.TagName, "tags_tag_name_key").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TagName)
                .HasMaxLength(200)
                .HasColumnName("tag_name");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "idx_users_role");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("user_id");
            entity.Property(e => e.AvatarId)
                .HasMaxLength(100)
                .HasColumnName("avatar_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(1)
                .HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<UserRoleEntity>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("user_roles_pkey");

            entity.ToTable("user_roles");

            entity.HasIndex(e => e.RoleName, "user_roles_role_name_key").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<UserStatisticEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_statistics_pkey");

            entity.ToTable("user_statistics");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.AverageReadingTime)
                .HasDefaultValue(0)
                .HasColumnName("average_reading_time");
            entity.Property(e => e.BooksRead)
                .HasDefaultValue(0)
                .HasColumnName("books_read");
            entity.Property(e => e.LastActive)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_active");

            entity.HasOne(d => d.User).WithOne(p => p.UserStatistic)
                .HasForeignKey<UserStatisticEntity>(d => d.UserId)
                .HasConstraintName("user_statistics_user_id_fkey");
        });

        modelBuilder.Entity<UserUnlockedEndingEntity>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.EndingId }).HasName("user_unlocked_endings_pkey");

            entity.ToTable("user_unlocked_endings");

            entity.HasIndex(e => e.EndingId, "idx_user_unlocked_endings_ending");

            entity.HasIndex(e => e.UserId, "idx_user_unlocked_endings_user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.EndingId).HasColumnName("ending_id");
            entity.Property(e => e.UnlockedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("unlocked_at");

            entity.HasOne(d => d.Ending).WithMany(p => p.UserUnlockedEndings)
                .HasForeignKey(d => d.EndingId)
                .HasConstraintName("user_unlocked_endings_ending_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UserUnlockedEndings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_unlocked_endings_user_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

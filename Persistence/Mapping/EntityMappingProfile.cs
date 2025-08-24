using AutoMapper;
using Core.Models;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Mapping
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            ConfigureUserMappings();
            ConfigureRefreshTokenMappings();
            ConfigureTagMappings();
            ConfigureGenreMappings();
            ConfigureSortOptionMappings();
            ConfigureBookmarkTypeMappings();
            ConfigureBookMappings();
        }

        private void ConfigureUserMappings()
        {
            // UserEntity → User
            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToDateTime(TimeOnly.MinValue)));

            // User → UserEntity
            CreateMap<User, UserEntity>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
                .ForMember(dest => dest.Bookmarks, opt => opt.Ignore())
                .ForMember(dest => dest.Books, opt => opt.Ignore())
                .ForMember(dest => dest.CommentLikes, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.IssueResponses, opt => opt.Ignore())
                .ForMember(dest => dest.Issues, opt => opt.Ignore())
                .ForMember(dest => dest.Notifications, opt => opt.Ignore())
                .ForMember(dest => dest.ReadingStatistics, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.UserStatistic, opt => opt.Ignore())
                .ForMember(dest => dest.UserUnlockedEndings, opt => opt.Ignore())
                .ForMember(dest => dest.BooksNavigation, opt => opt.Ignore());
        }
        private void ConfigureRefreshTokenMappings()
        {
            // RefreshTokenEntity → RefreshToken
            CreateMap<RefreshTokenEntity, RefreshToken>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            // RefreshToken → RefreshTokenEntity
            CreateMap<RefreshToken, RefreshTokenEntity>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        }
        private void ConfigureTagMappings()
        {
            CreateMap<TagEntity, Tag>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TagId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TagName));
            CreateMap<Tag, TagEntity>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TagName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
        private void ConfigureGenreMappings()
        {
            CreateMap<GenreEntity, Genre>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.GenreName));
            CreateMap<Genre, GenreEntity>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
        private void ConfigureSortOptionMappings()
        {
            CreateMap<SortOptionEntity, SortOption>()
                            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SortOptionId))
                            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<SortOption, SortOptionEntity>()
                .ForMember(dest => dest.SortOptionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Ignore())
                .ForMember(dest => dest.Description, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
        private void ConfigureBookmarkTypeMappings()
        {
            CreateMap<BookmarkTypeEntity, BookmarkType>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookmarkTypeId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.DisplayName));
        }
        private void ConfigureBookMappings()
        {
            CreateMap<BookEntity, Book>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
            .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.BookName))
            .ForMember(dest => dest.Annotation, opt => opt.MapFrom(src => src.Annotation))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.Username))
            .ForMember(dest => dest.CoverId, opt => opt.MapFrom(src => src.CoverId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.StatusName))
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.LanguageName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.IsAdultContent, opt => opt.MapFrom(src => src.IsAdultContent))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => (src.BookStatistic != null) ? src.BookStatistic.Rating : null))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
        
    }
}

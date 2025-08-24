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
    }
}

using Application.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Core.DTOs;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IMapper _mapper;
        private readonly SoeBackendContext _context;
        public BookRepository(IMapper mapper, SoeBackendContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public IQueryable<BookEntity> GetBooksQuery()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Status)
                .Include(b => b.Language)
                .Include(b => b.BookStatistic)
                .Include(b => b.Genres)
                .Include(b => b.Tags)
                .AsQueryable();
        }

        public async Task<PagedResult<Book>> GetBooksPagedAsync(BookFilter bookFilter, int page = 1, int pageSize = 32)
        {
            var query = GetBooksQuery();

            // Применяем фильтры
            query = ApplyFilters(query, bookFilter);

            // Применяем сортировку
            if (bookFilter is not null)
                query = ApplySorting(query, bookFilter.SortBy);

            // Получаем общее количество
            var totalCount = await query.CountAsync().ConfigureAwait(false);

            // Пагинация
            var booksEntities = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            // Маппим к Book
            var books = _mapper.Map<List<Book>>(booksEntities);

            return new PagedResult<Book>
            {
                Items = books,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private static IQueryable<BookEntity> ApplyFilters(IQueryable<BookEntity> query, BookFilter filter)
        {
            if (filter is null) return query;

            // Фильтр по тегам
            if (filter.IncludedTags is not null && filter.IncludedTags.Count > 0)
            {
                query = query.Where(b => b.Tags.Any(t => filter.IncludedTags.Contains(t.TagName)));
            }

            if (filter.ExcludedTags is not null && filter.ExcludedTags.Count > 0)
            {
                query = query.Where(b => !b.Tags.Any(t => filter.ExcludedTags.Contains(t.TagName)));
            }

            // Фильтр по жанрам
            if (filter.IncludedGenres is not null && filter.IncludedGenres.Count > 0)
            {
                query = query.Where(b => b.Genres.Any(g => filter.IncludedGenres.Contains(g.GenreName)));
            }

            if (filter.ExcludedGenres is not null && filter.ExcludedGenres.Count > 0)
            {
                query = query.Where(b => !b.Genres.Any(g => filter.ExcludedGenres.Contains(g.GenreName)));
            }

            // Фильтр по автору
            if (!string.IsNullOrEmpty(filter.AuthorName))
            {
                query = query.Where(b => b.Author.Username.Contains(filter.AuthorName));
            }

            // Фильтр по дате
            if (filter.StartDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(b => b.CreatedAt <= filter.EndDate.Value);
            }

            // Фильтр по названию
            if (!string.IsNullOrEmpty(filter.TitleFragment))
            {
                query = query.Where(b => b.BookName.Contains(filter.TitleFragment));
            }

            return query;
        }

        private static IQueryable<BookEntity> ApplySorting(IQueryable<BookEntity> query, string? sortBy)
        {
            return (sortBy?.ToLower()) switch
            {
                "popularity" => query.OrderByDescending(b => (b.BookStatistic != null) ? b.BookStatistic.FavoriteCount : 0),
                "views" => query.OrderByDescending(b => (b.BookStatistic != null) ? b.BookStatistic.ReadCount : 0),
                "newest" => query.OrderByDescending(b => b.CreatedAt),
                "rating" => query.OrderByDescending(b => (b.BookStatistic != null) ? b.BookStatistic.Rating : 0),
                "title" => query.OrderByDescending(b => b.BookName),
                _ => query.OrderByDescending(b => b.CreatedAt) // default
            };
        }
    }
}

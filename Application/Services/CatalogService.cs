using Application.DTOs;
using Application.Interfaces.CloudinaryService;
using Application.Interfaces.Repositories;
using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CatalogService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ICatalogDataRepository _catalogDataRepository;
        public CatalogService(
            IBookRepository bookRepository,
            IUserRepository userRepository,
            ICloudinaryService cloudinaryService,
            ICatalogDataRepository catalogDataRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
            _catalogDataRepository = catalogDataRepository;
        }
        public static (int start, int end) DataRange
        {
            get => (2020, DateTime.UtcNow.Year);
        }
        public async Task<PagedResult<BookCatalogDto>> GetCatalogAsync(
                BookFilter filter,
                Guid? userId = null,
                int page = 1,
                int pageSize = 32)
        {
            // 1. Получаем книги с фильтрацией и пагинацией
            var booksResult = await _bookRepository.GetBooksPagedAsync(filter, page, pageSize).ConfigureAwait(false);

            // 2. Проверяем возраст пользователя
            var isMinor = await IsUserMinor(userId).ConfigureAwait(false);

            // 3. Маппим в DTO с учетом возрастных ограничений
            var items = booksResult.Items.Select(book => MapToCatalogDto(book, isMinor)).ToList();

            return new PagedResult<BookCatalogDto>
            {
                Items = items,
                TotalCount = booksResult.TotalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await _catalogDataRepository.GetTagsAsync().ConfigureAwait(false);
        }
        public async Task<List<Genre>> GetGenresAsync()
        {
            return await _catalogDataRepository.GetGenresAsync().ConfigureAwait(false);
        }
        public async Task<List<SortOption>> GetSortOptionsAsync()
        {
            return await _catalogDataRepository.GetSortOptionsAsync().ConfigureAwait(false);
        }
        
        private async ValueTask<bool> IsUserMinor(Guid? userId)
        {
            if (!userId.HasValue) 
                return true; // Неавторизованный = считаем несовершеннолетним

            var user = await _userRepository.GetByIdAsync(userId.Value).ConfigureAwait(false);

            if (user is null)
                throw new InvalidOperationException("User not exist!");

            return (DateTime.Today.Year - user.DateOfBirth.Year) < 18;
        }
        private BookCatalogDto MapToCatalogDto(Book book, bool isMinor)
        {
            // Impossible
            if (book is null)
                throw new ArgumentException(null, nameof(book));

            var coverUrl = !string.IsNullOrEmpty(book.CoverId)
                ? _cloudinaryService.GetImageUrl(
                    book.CoverId,
                    600,
                    600,
                    isBlurred: isMinor && book.IsAdultContent,
                    blurStrength: 1000)
                : _cloudinaryService.GetImageUrl("AntoniusBlok_kwz0pg", 600, 600);

            return new BookCatalogDto
            {
                Id = book.BookId,
                Title = book.BookName,
                Description = book.Description,
                CoverUrl = coverUrl,
                Rating = book.Rating ?? 0,
                IsAgeRestricted = book.IsAdultContent
            };
        }
    }
}

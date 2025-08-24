using Core.DTOs;
using SOEBackend.Contracts.Catalog;

namespace SOEBackend.Mappings
{
    internal static class CatalogRequestExtensions
    {
        public static BookFilter ToBookFilter(this CatalogRequest request)
        {
            return new BookFilter
            {
                SortBy = request.SortBy,
                IncludedTags = ParseStringToList(request.IncludedTags),
                ExcludedTags = ParseStringToList(request.ExcludedTags),
                IncludedGenres = ParseStringToList(request.IncludedGenres),
                ExcludedGenres = ParseStringToList(request.ExcludedGenres),
                AuthorName = request.Author,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TitleFragment = request.TitleFragment
            };
        }

        private static List<string>? ParseStringToList(string? input)
        {
            return !string.IsNullOrEmpty(input)
                ? input.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                : null;
        }
    }
}

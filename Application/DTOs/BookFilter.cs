using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class BookFilter
    {
        public List<string>? IncludedTags { get; set; }
        public List<string>? ExcludedTags { get; set; }
        public List<string>? IncludedGenres { get; set; }
        public List<string>? ExcludedGenres { get; set; }
        public string? AuthorName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? TitleFragment { get; set; }
        public string? SortBy { get; set; }

        // Для удобства
        public bool IsEmpty =>
            (IncludedTags == null || !IncludedTags.Any()) &&
            (ExcludedTags == null || !ExcludedTags.Any()) &&
            (IncludedGenres == null || !IncludedGenres.Any()) &&
            (ExcludedGenres == null || !ExcludedGenres.Any()) &&
            string.IsNullOrEmpty(AuthorName) &&
            StartDate == null &&
            EndDate == null &&
            string.IsNullOrEmpty(TitleFragment);
    }
}

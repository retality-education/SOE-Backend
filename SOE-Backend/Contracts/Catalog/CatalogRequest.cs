namespace SOEBackend.Contracts.Catalog
{
    internal record CatalogRequest(
        int Page = 1,
        int PageSize = 32,
        string? SortBy = "newest",
        string? IncludedTags = null,
        string? ExcludedTags = null,
        string? IncludedGenres = null,
        string? ExcludedGenres = null,
        string? Author = null,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        string? TitleFragment = null);
}

using Application.Services;
using SOEBackend.Contracts.Catalog;
using SOEBackend.Mappings;
using System.Security.Claims;

namespace SOEBackend.Endpoints
{
    internal static class CatalogEndpoints
    {
        public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("api/catalog");

            endpoints.MapGet("filters", GetFilters);
            endpoints.MapGet(string.Empty, GetCatalog);

            return endpoints;
        }

        private static async Task<IResult> GetCatalog(
            [AsParameters] CatalogRequest catalogRequest,
            HttpContext httpContext, 
            CatalogService catalogService)
        {
            Guid? userId = null;

            var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userIdClaim))
                if (Guid.TryParse(userIdClaim, out var parsedUserId))
                    userId = parsedUserId;

            var catalog = await catalogService.GetCatalogAsync(
                catalogRequest.ToBookFilter(),
                userId, 
                catalogRequest.Page,
                catalogRequest.PageSize
                ).ConfigureAwait(false);

            return Results.Ok(catalog);
        }

        private static async Task<IResult> GetFilters(CatalogService catalogService)
        {
            var tags = await catalogService.GetTagsAsync().ConfigureAwait(false);
            var genres = await catalogService.GetGenresAsync().ConfigureAwait(false);
            var datesRange = new { CatalogService.DataRange.start, CatalogService.DataRange.end };
            var sortOptions = await catalogService.GetSortOptionsAsync().ConfigureAwait(false);

            return Results.Ok(new { tags, genres, sortOptions, datesRange  });
        }
    }
}

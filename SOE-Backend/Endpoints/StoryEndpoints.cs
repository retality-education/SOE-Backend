using Microsoft.AspNetCore.Mvc;

namespace SOEBackend.Endpoints
{
    internal static class StoryEndpoints
    {
        public static IEndpointRouteBuilder MapStoryEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("story").RequireAuthorization();

            endpoints.MapGet(string.Empty, GetStory);

            return endpoints;
        }

        private static async Task<IResult> GetStory()
        {
            return Results.Ok("hello world");
        }
    }
}

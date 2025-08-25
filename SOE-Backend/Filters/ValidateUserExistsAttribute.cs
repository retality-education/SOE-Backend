using Application.Services;
using System.Security.Claims;

namespace SOEBackend.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class ValidateUserExistsAttribute : Attribute, IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;
            var validationService = context.HttpContext.RequestServices.GetService<ValidationService>()!;

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Results.Unauthorized();

            if (!Guid.TryParse(userId, out var id))
                return Results.Unauthorized();

            if (!await validationService.IsUserExistById(id).ConfigureAwait(false))
                return Results.NotFound("User not found");

            return await next(context).ConfigureAwait(false);
        }
    }
}

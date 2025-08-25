using Application.Interfaces.Auth;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace SOEBackend.CustomAttributes
{
    internal class ValidateRefreshTokenFilter : IEndpointFilter
    {
        private readonly IJwtProvider _jwtProvider;

        public ValidateRefreshTokenFilter(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Console.WriteLine("ValidateRefreshTokenFilter called!");

            var httpContext = context.HttpContext;

            var refreshToken = await GetRefreshTokenFromBody(httpContext.Request)
                             ?? httpContext.Request.Cookies["meow-cookie"]
                             ?? httpContext.Request.Query["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Results.Unauthorized();
            }

            try
            {
                var principal = _jwtProvider.ValidateToken(refreshToken, true);
                httpContext.Items["ValidatedRefreshToken"] = refreshToken;
            }
            catch
            {
                return Results.Unauthorized();
            }

            return await next(context);
        }

        private async Task<string?> GetRefreshTokenFromBody(HttpRequest request)
        {
            if (!request.HasJsonContentType()) return null;

            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            try
            {
                var json = JsonDocument.Parse(body);
                return json.RootElement.GetProperty("refreshToken").GetString();
            }
            catch
            {
                return null;
            }
        }
    }
}

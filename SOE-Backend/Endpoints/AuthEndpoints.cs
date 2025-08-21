using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Routing;
using SOE_Backend.Contracts.Authentication;
using SOE_Backend.Contracts.Authethication;
using SOE_Backend.Contracts.Users;
using SOE_Backend.CustomAttributes;
using System.Net.Http;

namespace SOE_Backend.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("api/auth");

            endpoints.MapPost("register", Register);

            endpoints.MapPost("login", Login);

            endpoints.MapPost("refresh", RefreshToken).AddEndpointFilter<ValidateRefreshTokenFilter>(); ;

            endpoints.MapPost("logout", Logout).AddEndpointFilter<ValidateRefreshTokenFilter>(); ;

            return app;
        }
        private static async Task<IResult> Register(
            RegisterUserRequest request,
            AuthService authService)
        {
            await authService.Register(request.UserName, request.Email, request.Password);

            return Results.Ok();
        }

        private static async Task<IResult> Login(
            LoginUserRequest request,
            AuthService authService,
            HttpContext httpContext)
        {
            var (token, rtoken) = await authService.Login(request.Email, request.Password);

            httpContext.Response.Cookies.Append("cool-coocka", token);

            httpContext.Response.Cookies.Append("meow-cookie", rtoken);

            return Results.Ok(token);
        }
        private static async Task<IResult> RefreshToken(
            RefreshTokenRequest request,
            AuthService authService,
            HttpContext httpContext)
        {
            try
            {
                var refreshToken = httpContext.Items["ValidatedRefreshToken"] as string;

                if (refreshToken is null)
                    throw new Exception("Refresh token not found!");

                var tokens = await authService.RefreshAccessToken(refreshToken);

                httpContext.Response.Cookies.Delete("cool-coocka");
                httpContext.Response.Cookies.Delete("meow-cookie");

                httpContext.Response.Cookies.Append("cool-coocka", tokens.AccessToken);
                httpContext.Response.Cookies.Append("meow-cookie", tokens.RefreshToken);

                return Results.Ok(new { tokens.AccessToken, tokens.RefreshToken});

            }
            catch (Exception)
            {
                return Results.Unauthorized();
            }
        }
        private static async Task<IResult> Logout(
            LogoutRequest logoutRequest,
            AuthService authService,
            HttpContext httpContext
            )
        {
            var refreshToken = httpContext.Items["ValidatedRefreshToken"] as string;

            if (refreshToken is null)
                throw new Exception("Refresh token not found!");

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await authService.Logout(refreshToken);
            }
 
            httpContext.Response.Cookies.Delete("cool-coocka");
            httpContext.Response.Cookies.Delete("meow-cookie");

            return Results.Ok(new { message = "Succeed logout!" });
        }
    }
}

using Application.Extensions;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SOEBackend.Contracts.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ForgotPasswordRequest = SOEBackend.Contracts.Users.ForgotPasswordRequest;
using ResetPasswordRequest = SOEBackend.Contracts.Users.ResetPasswordRequest;

namespace SOEBackend.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var endpoints = app.MapGroup("api/user");

            endpoints.MapGet("me", Me).RequireAuthorization();
            endpoints.MapPost("forgot-password", ForgotPassword);
            endpoints.MapPost("reset-password", ResetPassword);
            endpoints.MapPost("change-password", ChangePassword).RequireAuthorization();
            endpoints.MapPost("change-avatar", ChangeAvatar).DisableAntiforgery().RequireAuthorization();

            return app;
        }

        private static async Task<IResult> Me(
            HttpContext httpContext,
            UserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                throw new Exception("Token in invalid!");

            var user = await userService.Me(userId);
            return Results.Ok(new
            {
                userId = user.Id,
                userName = user.UserName,
                email = user.Email,
                avatarUrl = user.AvatarId
            });
        }

        private static async Task<IResult> ForgotPassword(
            ForgotPasswordRequest request,
            UserService userService)
        {
            await userService.ForgotPassword(request.Email);
            return Results.Ok(new { Message = "If email exists, reset code has been sent" });
        }

        private static async Task<IResult> ResetPassword(
            ResetPasswordRequest request,
            UserService userService)
        {
            await userService.ResetPassword(request.Email, request.ResetCode, request.NewPassword);
            return Results.Ok(new { Message = "Password successfully reset" });
        }

        private static async Task<IResult> ChangePassword(
            ChangePasswordRequest request,
            HttpContext httpContext,
            UserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                throw new Exception("Token in invalid!");

            await userService.ChangePassword(userId, request.CurrentPassword, request.NewPassword);
            return Results.Ok(new { Message = "Password successfully changed" });
        }


        private static async Task<IResult> ChangeAvatar(
            [FromForm] ChangeAvatarRequest request,
            HttpContext httpContext,
            UserService userService)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                throw new Exception("Token in invalid!");

            var avatarUrl = await userService.ChangeAvatar(userId, request.Avatar);
            return Results.Ok(new { AvatarUrl = avatarUrl });
        }
    }
}
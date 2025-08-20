using Application.Contracts.RestoreCode;
using Application.Interfaces.Auth;
using Application.Interfaces.Caching;
using Application.Interfaces.CloudinaryService;
using Application.Interfaces.EmailSender;
using Application.Interfaces.Repositories;
using Application.Interfaces.RestoreCode;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IEmailSender _emailSender;
        private readonly ICacheService _cacheService;
        private readonly IRestoreCodeProvider _restoreCodeProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        public UserService(IUsersRepository usersRepository, 
            ICloudinaryService cloudinaryService, 
            IEmailSender emailSender, 
            ICacheService cacheService,
            IRestoreCodeProvider restoreCodeProvider,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)

        {
            _usersRepository = usersRepository;
            _cloudinaryService = cloudinaryService;
            _emailSender = emailSender;
            _cacheService = cacheService;
            _restoreCodeProvider = restoreCodeProvider;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }
        public async Task<User> Me(Guid userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);

            if (user.AvatarId is not null)
                user.AvatarId = _cloudinaryService.GetImageUrl(user.AvatarId, 600, 600);

            return user;
        }
        public async Task<string> ChangeAvatar(Guid userId, IFormFile avatarFile)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                throw new ArgumentException("Avatar file is required");

            if (avatarFile.Length > 5 * 1024 * 1024) // 5MB limit
                throw new ArgumentException("Avatar file size must be less than 5MB");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(avatarFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only JPG, PNG and GIF images are allowed");

            var uploadResult = await _cloudinaryService.UploadAvatarAsync(avatarFile, userId);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Failed to upload avatar");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            user.AvatarId = uploadResult.PublicId.ToString();
            await _usersRepository.UpdateAsync(user);

            return _cloudinaryService.GetImageUrl(uploadResult.PublicId, 200, 200);
        }
        public async Task ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required");

            if (newPassword.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            if (!_passwordHasher.Verify(currentPassword, user.PasswordHash))
                throw new InvalidOperationException("Current password is incorrect");

            var newPasswordHash = _passwordHasher.Generate(newPassword);
            user.PasswordHash = newPasswordHash;

            await _usersRepository.UpdateAsync(user);
        }
        public async Task ForgotPassword(string email)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            if (user == null)
                return;

            var restoreCode = _restoreCodeProvider.GenerateRestoreCode();
            var cacheKey = $"password_reset_{email}";

            await _cacheService.SetAsync(cacheKey, new PasswordResetData
            {
                Code = restoreCode,
                Email = email,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            }, TimeSpan.FromMinutes(30));

            await _emailSender.SendPasswordResetCodeAsync(email, restoreCode, user.UserName);
        }
        public async Task ResetPassword(string email, string resetCode, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                throw new ArgumentException("Invalid new password");

            var cacheKey = $"password_reset_{email}";
            var resetData = await _cacheService.GetAsync<PasswordResetData>(cacheKey);

            if (resetData == null || resetData.Code != resetCode || resetData.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("Invalid or expired reset code");

            var user = await _usersRepository.GetByIdAsync(resetData.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            var newPasswordHash = _passwordHasher.Generate(newPassword);
            user.PasswordHash = newPasswordHash;
            await _usersRepository.UpdateAsync(user);

            await _cacheService.RemoveAsync(cacheKey);
        }
        public Guid GetUserIdFromCookie(string cookie)
        {
            var userId = _jwtProvider.GetUserIdFromAccessTokem(cookie);
            if (userId is null)
                throw new Exception("Cookie is invalid!");

            return (Guid)userId;
        }

        //public async Task GetFavoriets()
        //{

        //}
        //public async Task GetBooksByBookmark()
        //{

        //}
    }
}

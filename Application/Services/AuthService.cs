using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IRefreshTokenRepository refreshTokenRepository,
            IUsersRepository usersRepository, 
            IPasswordHasher passwordHasher, 
            IJwtProvider jwtProvider)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }


        public async Task Register(string userName, string email, string password)
        {
            if (await _usersRepository.ExistsByEmailAsync(email))
            {
                throw new ArgumentException("User with this email already exists", nameof(email));
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

            await _usersRepository.AddAsync(user);
        }
        public async Task<(string AccessToken, string RefreshToken)> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmailAsync(email);
            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = _jwtProvider.GenerateToken(user);

            var rtoken = _jwtProvider.GenerateRefreshToken(user);

            await _refreshTokenRepository.SaveRefreshTokenAsync(rtoken);

            return (token, rtoken.Token);
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshAccessToken(string refreshToken)
        {
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired refresh token");

            var user = await _usersRepository.GetByIdAsync(storedToken.UserId);

            // Генерируем новые токены
            var newAccessToken = _jwtProvider.GenerateToken(user);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken(user);

            // Обновляем токены в хранилище
            await _refreshTokenRepository.ReplaceAsync(storedToken.Id, newRefreshToken);

            return (newAccessToken, newRefreshToken.Token);
        }

        public async Task Logout(string refreshToken)
        {
            var tokenId = _jwtProvider.GetRefreshTokenId(refreshToken);
            if (tokenId is null)
                throw new Exception("Refresh token is invalid");
            await _refreshTokenRepository.RevokeAsync((Guid)tokenId);
        }

    }
}

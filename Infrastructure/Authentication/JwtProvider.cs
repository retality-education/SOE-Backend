using Application.Extensions;
using Application.Interfaces.Auth;
using Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_options.SecretKey))
                throw new ArgumentException("JWT secret key is not configured");
        }

        public string GenerateToken(User user)
        {
            return GenerateJwtToken(
                user,
                _options.SecretKey,
                TimeSpan.FromHours(_options.ExpiresHours));
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var jti = Guid.NewGuid();
            return new RefreshToken
            {
                Id = jti,
                Token = GenerateJwtToken(
                    user,
                    _options.RefreshSecretKey,
                    TimeSpan.FromDays(_options.RefreshExpiresDays),
                    new Claim(JwtRegisteredClaimNames.Jti, jti.ToString())
                ),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshExpiresDays),
                IsRevoked = false
            };
        }

        public Guid? GetRefreshTokenId(string refreshToken)
        {
            try
            {
                var claims = GetClaimsFromToken(refreshToken, true);
                var jtiClaim = claims?.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

                if (jtiClaim != null && Guid.TryParse(jtiClaim.Value, out var jti))
                {
                    return jti;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private string GenerateJwtToken(User user, string secretKey, TimeSpan lifetime, params Claim[] additionalClaims)
        {
            var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("userId", user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(additionalClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal? ValidateToken(string token, bool isRefreshToken = false)
        {
            try
            {
                var secretKey = isRefreshToken ? _options.RefreshSecretKey : _options.SecretKey;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public Guid? GetUserIdFromAccessTokem(string token)
        {
            try
            {
                var claims = GetClaimsFromToken(token, false);
                var subClaim = claims?.GetUserId();

                if (subClaim is not null && Guid.TryParse(subClaim, out var sub))
                    return sub;
                return null;
            }
            catch
            {
                return null;
            }
        }
        private IEnumerable<Claim>? GetClaimsFromToken(string token, bool isRefreshToken = false)
        {
            var principal = ValidateToken(token, isRefreshToken);
            return principal?.Claims;
        }
    }
}

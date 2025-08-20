using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ClaimsExtensions
    {
        public static string? GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            return claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
        public static string? GetJti(this IEnumerable<Claim> claims)
        {
            return claims.GetClaimValue(JwtRegisteredClaimNames.Jti);
        }
        public static string? GetUsername(this IEnumerable<Claim> claims)
        {
            return claims.GetClaimValue("username");
        }

        public static string? GetEmail(this IEnumerable<Claim> claims)
        {
            return claims.GetClaimValue(JwtRegisteredClaimNames.Email);
        }

        public static string? GetUserId(this IEnumerable<Claim> claims)
        {
            return claims.GetClaimValue("userId");
        }
    }
}

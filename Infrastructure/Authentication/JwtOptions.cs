namespace Infrastructure.Authentication
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresHours { get; set; }
        public string RefreshSecretKey { get; set; } = string.Empty;
        public int RefreshExpiresDays { get; set; } = 30;
    }
}
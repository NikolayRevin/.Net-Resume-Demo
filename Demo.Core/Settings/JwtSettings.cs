using System;
namespace Demo.Core.Settings
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        public string SqlConnectionString { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
        public int TokenExpiryTimeInSeconds { get; set; }
        public int RefreshTokenExpiryTimeInDays { get; set; }
    }
}

using System;
namespace Demo.Core.Models.Auth
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}

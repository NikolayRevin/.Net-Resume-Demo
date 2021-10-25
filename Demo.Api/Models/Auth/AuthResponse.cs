using System;
using Demo.Core.Models.Auth;

namespace Demo.Api.Models.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

        public AuthResponse(AuthResult result)
        {
            Token = result.Token;
            TokenType = result.TokenType;
            RefreshToken = result.RefreshToken;
            Expiration = result.Expiration;
        }
    }
}

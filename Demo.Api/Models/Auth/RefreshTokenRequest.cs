using System;
using Demo.Core.Exceptions;
using Demo.Core.Models.Auth;

namespace Demo.Api.Models.Auth
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public RefreshTokenAction ToAction()
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                throw new BadRequestApiException("Field RefreshToken should not be empty");
            }

            var action = new RefreshTokenAction
            {
                RefreshToken = RefreshToken
            };

            return action;
        }
    }
}

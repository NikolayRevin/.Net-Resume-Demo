using System;
namespace Demo.Core.Exceptions
{
    public class InvalidateRefreshTokenApiException: BaseApiException
    {
        public InvalidateRefreshTokenApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40012;
            Error = "BAD_REQUEST";
            ErrorMessage = "Токен не валидный. Авторизуйтесь заново.";
            Description = description;
        }
    }
}

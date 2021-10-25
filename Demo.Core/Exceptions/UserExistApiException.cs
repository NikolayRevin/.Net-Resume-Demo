using System;
namespace Demo.Core.Exceptions
{
    public class UserExistApiException: BaseApiException
    {
        public UserExistApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40010;
            Error = "BAD_REQUEST";
            ErrorMessage = "Пользователь с таким email существует";
            Description = description;
        }
    }
}

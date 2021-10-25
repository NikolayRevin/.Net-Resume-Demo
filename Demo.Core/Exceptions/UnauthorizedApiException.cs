using System;
namespace Demo.Core.Exceptions
{
    public class UnauthorizedApiException: BaseApiException
    {
        public UnauthorizedApiException(string description = null)
        {
            HttpCode = 401;
            ErrorCode = 40100;
            Error = "UNAUTHRIZED";
            ErrorMessage = "Не верный email или пароль.";
            Description = description;
        }
    }
}

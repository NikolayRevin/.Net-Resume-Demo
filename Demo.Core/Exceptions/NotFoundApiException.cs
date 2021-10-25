using System;
namespace Demo.Core.Exceptions
{
    public class NotFoundApiException: BaseApiException
    {
        public NotFoundApiException(string description = null)
        {
            HttpCode = 404;
            ErrorCode = 40400;
            Error = "NOT_FOUND";
            ErrorMessage = "Запрашиваемый объкт не найден.";
            Description = description;
        }
    }
}

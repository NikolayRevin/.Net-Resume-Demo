using System;
namespace Demo.Core.Exceptions
{
    public class UnknownApiException: BaseApiException
    {
        public UnknownApiException(string description = null)
        {
            HttpCode = 500;
            ErrorCode = 50000;
            Error = "UNKNOWN";
            ErrorMessage = "Что-то пошло не так. Повторите запрос позднее.";
            Description = description;
        }
    }
}

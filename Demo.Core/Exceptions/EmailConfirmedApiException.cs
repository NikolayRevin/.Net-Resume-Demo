using System;
namespace Demo.Core.Exceptions
{
    public class EmailConfirmedApiException: BaseApiException
    {
        public EmailConfirmedApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40050;
            Error = "EMAIL_CONFIRMED";
            ErrorMessage = "E-mail уже подтвержден.";
            Description = description;
        }
    }
}

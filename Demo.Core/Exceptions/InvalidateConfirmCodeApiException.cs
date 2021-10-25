using System;
namespace Demo.Core.Exceptions
{
    public class InvalidateConfirmCodeApiException: BaseApiException
    {
        public InvalidateConfirmCodeApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40013;
            Error = "INVALIDATE_CONFIRM_CODE";
            ErrorMessage = "Неверный код подтверждения";
            Description = description;
        }
    }
}

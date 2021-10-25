using System;

namespace Demo.Core.Exceptions
{
	public class ChangePasswordApiException: BaseApiException
	{
		public ChangePasswordApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40020;
            Error = "BAD_REQUEST";
            ErrorMessage = "Не удалось сменить пароль";
            Description = description;
        }
    }
}

namespace Demo.Core.Exceptions
{
    public class BadRequestApiException: BaseApiException
    {
        public BadRequestApiException(string description = null)
        {
            HttpCode = 400;
            ErrorCode = 40000;
            Error = "BAD_REQUEST";
            ErrorMessage = "Заполнены не все обязательные параметры";
            Description = description;
        }
    }
}

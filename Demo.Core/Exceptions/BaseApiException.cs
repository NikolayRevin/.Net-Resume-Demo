using System;

namespace Demo.Core.Exceptions
{
    public abstract class BaseApiException : Exception
    {
        public int HttpCode { get; set; } = 500;

        public string Error { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Description { get; set; }        
    }
}

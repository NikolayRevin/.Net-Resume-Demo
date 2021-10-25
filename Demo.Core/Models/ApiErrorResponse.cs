using System;
using Demo.Core.Exceptions;

namespace Demo.Core.Models
{
    public class ApiErrorResponse
    {
        public string Error { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Description { get; set; }

        public ApiErrorResponse(BaseApiException exp)
        {
            Error = exp.Error;
            ErrorCode = exp.ErrorCode;
            ErrorMessage = exp.ErrorMessage;
            Description = exp.Description;
        }
    }
}

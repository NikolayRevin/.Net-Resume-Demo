using Demo.Core.Exceptions;
using Demo.Core.Models.Auth;

namespace Demo.Api.Models.Auth
{
    public class RestorePasswordRequest
    {
        public string Email { get; set; }

        public RestorePasswordAction ToAction()
        {
            if (string.IsNullOrEmpty(Email))
            {
                throw new BadRequestApiException("Field Email should not be empty");
            }

            var action = new RestorePasswordAction
            {
                Email = Email
            };

            return action;
        }
    }
}

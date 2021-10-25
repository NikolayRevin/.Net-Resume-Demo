using Demo.Core.Exceptions;
using Demo.Core.Models.Auth;

namespace Demo.Api.Models.Auth
{
    public class LoginRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public LoginAction ToAction()
        {
            if (string.IsNullOrEmpty(Login))
            {
                throw new BadRequestApiException("Field Login should not be empty");
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new BadRequestApiException("Field Password should not be empty");
            }

            var action = new LoginAction
            {
                Login = Login,
                Password = Password
            };

            return action;
        }
    }
}

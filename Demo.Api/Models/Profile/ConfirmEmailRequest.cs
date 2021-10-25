using Demo.Core.Models.Profile;
using Demo.Core.Exceptions;

namespace Demo.Api.Models.Profile
{
    public class ConfirmEmailRequest
    {
        public string Email { get; set; }

        public string Code { get; set; }

        public ConfirmEmailAction ToAction(string id)
        {

            if (string.IsNullOrEmpty(Code))
            {
                throw new BadRequestApiException("Field Code should not be empty");
            }

            if (string.IsNullOrEmpty(Email))
            {
                throw new BadRequestApiException("Field Email should not be empty");
            }

            var action = new ConfirmEmailAction
            {
                UserId = id,
                Code = Code,
                Email = Email
            };

            return action;
        }
    }
}

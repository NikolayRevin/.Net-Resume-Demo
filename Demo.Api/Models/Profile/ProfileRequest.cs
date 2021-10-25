using System;
using Demo.Core.Models.Profile;
using Demo.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Demo.Api.Models.Profile
{
    public class ProfileRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool SendPush { get; set; }

        public ProfileUpdateAction ToAction(string id)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new BadRequestApiException("Field Name should not be empty");
            }

            if (string.IsNullOrEmpty(Phone))
            {
                throw new BadRequestApiException("Field Phone should not be empty");
            }

            if (string.IsNullOrEmpty(Email))
            {
                throw new BadRequestApiException("Field Email should not be empty");
            }

            if (! string.IsNullOrEmpty(Password))
            {
                if (Password.Length < 6)
                {
                    throw new BadRequestApiException("Field Password be more than 6 characters");
                }
                throw new BadRequestApiException("Field Password should not be empty");
            }

            var emailAddressAttr = new EmailAddressAttribute();

            if (!emailAddressAttr.IsValid(Email))
            {
                throw new BadRequestApiException("Field Email not valid");
            }

            var action = new ProfileUpdateAction
            {
                UserId = id,
                Name = Name,
                Email = Email,
                Phone = Phone,
                Password = Password,
                Description = Description,
                SendPush = SendPush
            };
            
            return action;
        }
    }
}

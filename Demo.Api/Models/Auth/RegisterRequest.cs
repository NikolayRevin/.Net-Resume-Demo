using System;
using System.ComponentModel.DataAnnotations;
using Demo.Core.Exceptions;
using Demo.Core.Models.Auth;

namespace Demo.Api.Models.Auth
{
    public class RegisterRequest
    {       
        public string Name { get; set; }
      
        public string Email { get; set; }
      
        public string Phone { get; set; }

        public string Password { get; set; }

        public RegisterAction ToAction()
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

            if (string.IsNullOrEmpty(Password))
            {
                throw new BadRequestApiException("Field Password should not be empty");
            }

            var emailAddressAttr = new EmailAddressAttribute();

            if (!emailAddressAttr.IsValid(Email))
            {
                throw new BadRequestApiException("Field Email not valid");
            }

            if (Password.Length < 6)
            {
                throw new BadRequestApiException("Field Password be more than 6 characters");
            }

            var action = new RegisterAction
            {
                Name = Name,
                Email = Email,
                Phone = Phone,
                Password = Password
            };

            return action;
        }
    }
}

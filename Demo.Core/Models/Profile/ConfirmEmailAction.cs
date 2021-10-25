using System;
namespace Demo.Core.Models.Profile
{
    public class ConfirmEmailAction
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }
    }
}

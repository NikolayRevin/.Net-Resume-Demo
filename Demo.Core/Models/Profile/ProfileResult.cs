using System;
using Demo.Core.DAL.Entities;
using Demo.Core.Models.User;

namespace Demo.Core.Models.Profile
{
    public class ProfileResult: UserResult
    {
        public bool SendPush { get; set; }

        public bool EmailConfirmed { get; set; }

        public ProfileResult(UserEntity user) : base(user)
        {
            SendPush = user.SendPush;
            EmailConfirmed = user.EmailConfirmed;
        }
    }
}

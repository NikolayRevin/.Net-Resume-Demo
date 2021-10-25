using System;
using Demo.Core.DAL.Entities;

namespace Demo.Core.Models.User
{
    public class UserResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? Vote { get; set; }

        public UserResult(UserEntity user)
        {
            Id = user.Id;
            Name = user.Name;
            Description = user.Description;
            Email = user.Email;
            Phone = user.PhoneNumber;
            Vote = user.Vote?.Vote;
        }
    }
}

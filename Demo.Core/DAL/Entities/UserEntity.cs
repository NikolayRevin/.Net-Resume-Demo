using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Demo.Core.DAL.Entities
{
    public class UserEntity: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool SendPush { get; set; }

        public virtual UserVoteEntity Vote { get; set; }

        public List<RefreshTokenEntity> RefreshTokens { get; set; }
        
        public string ConfirmCode { get; set; }
        
        public DateTime ConfirmDate { get; set; }
    }
}

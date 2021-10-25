using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Core.DAL.Entities
{
    public class RefreshTokenEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public string Id { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }

        public UserEntity User { get; set; }

        public DateTime ExpiryOn { get; set; }
    }
}

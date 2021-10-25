using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Core.DAL.Entities
{
    public class UserVoteEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        [Required]
        public string FromUserId { get; set; }
        public virtual UserEntity FromUser { get; set; }
        [Required, Range(1, 5)]
        public int Vote { get; set; }
    }
}

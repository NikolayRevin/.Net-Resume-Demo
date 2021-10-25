using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Core.DAL.Entities
{
    public class DeviceEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public string Id { get; set; }
        public virtual UserEntity User { get; set; }
        [Required]
        public string DevicePlatform { get; set; }
        [Required]
        public string FcmToken { get; set; }
    }
}

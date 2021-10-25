using System;
namespace Demo.Core.Models.Profile
{
    public class AddDeviceAction
    {
        public string UserId { get; set; }

        public string DevicePlatform { get; set; }

        public string FcmToken { get; set; }

    }
}

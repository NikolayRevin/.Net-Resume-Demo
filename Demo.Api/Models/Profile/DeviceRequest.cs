using Demo.Core.Exceptions;
using Demo.Core.Models.Profile;

namespace Demo.Api.Models.Profile
{
    public class DeviceRequest
    {
        public string DevicePlatform { get; set; }

        public string FcmToken { get; set; }

        public AddDeviceAction ToAction(string id)
        {
            if (string.IsNullOrEmpty(DevicePlatform))
            {
                throw new BadRequestApiException("Field DevicePlatform should not be empty");
            }

            if (string.IsNullOrEmpty(FcmToken))
            {
                throw new BadRequestApiException("Field FcmToken should not be empty");
            }

            var action = new AddDeviceAction
            {
                UserId = id,
                DevicePlatform = DevicePlatform,
                FcmToken = FcmToken
            };

            return action;
        }
    }
}

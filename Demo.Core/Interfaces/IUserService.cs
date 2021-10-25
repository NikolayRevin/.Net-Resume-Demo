using System;
using System.Threading.Tasks;
using Demo.Core.Models.User;
using Demo.Core.Models.Profile;

namespace Demo.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserResult[]> GetUserListAsync();
        Task<T> GetUserAsync<T>(string id);
        Task<ProfileResult> UpdateUserAsync(ProfileUpdateAction action);
        Task RefreshUserConfirmCodeAsync(string id);
        Task<ProfileResult> ConfirmEmailAsync(ConfirmEmailAction action);
        Task AddDeviceAsync(AddDeviceAction action);
        Task<UserResult> VoteForUserAsync(UserVoteAction action);
    }
}

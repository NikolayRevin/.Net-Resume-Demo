using System.Threading.Tasks;
using Demo.Core.DAL;
using Demo.Core.DAL.Entities;
using Demo.Core.Interfaces;
using Demo.Core.Models.Profile;
using Demo.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Demo.Core.Helpers;
using System;
using Demo.Core.Models.User;
using Hangfire;

namespace Demo.Core.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly DbManager _dbManager;
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public UserService(UserManager<UserEntity> userManager, DbManager dbManager, IMailService mailService, IBackgroundJobClient backgroundJobClient)
        {
            _userManager = userManager;
            _dbManager = dbManager;
            _mailService = mailService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<UserResult[]> GetUserListAsync()
        {
            var dbUsers = await _dbManager.Users.ToArrayAsync();
            var users = new UserResult[dbUsers.Length];

            for (var i = 0; i < dbUsers.Length; i++)
            {
                users[i] = new UserResult(dbUsers[i]);
            }

            return users;
        }

        public async Task<T> GetUserAsync<T>(string id)
        {
            var user = await GetUserEntityAsync(id);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            return (T)Activator.CreateInstance(typeof(T), user);
        }

        public async Task<ProfileResult> UpdateUserAsync(ProfileUpdateAction action)
        {
            var user = await GetUserEntityAsync(action.UserId);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            bool EmailChanged = user.Email != action.Email;
            if (EmailChanged)
            {
                user.EmailConfirmed = false;
                user.ConfirmCode = StringHelper.GenerateRandomCode();
            }

            user.Name = action.Name;
            user.Description = action.Description;
            user.Email = action.Email;
            user.PhoneNumber = action.Phone;
            user.SendPush = action.SendPush;

            await _dbManager.SaveChangesAsync();

            if (!string.IsNullOrEmpty(action.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, action.Password);

                if (!result.Succeeded)
                    throw new ChangePasswordApiException();
            }

            if (EmailChanged)
            {
                _backgroundJobClient.Enqueue(() => _mailService.SendConfirmCode(user.Email, user.ConfirmCode));
            }

            return new ProfileResult(user);
        }

        public async Task RefreshUserConfirmCodeAsync(string id)
        {
            var user = await GetUserEntityAsync(id);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            if (user.EmailConfirmed)
                throw new EmailConfirmedApiException();

            user.ConfirmCode = StringHelper.GenerateRandomCode();
            await _dbManager.SaveChangesAsync();

            _backgroundJobClient.Enqueue(() => _mailService.SendConfirmCode(user.Email, user.ConfirmCode));
        }

        public async Task<ProfileResult> ConfirmEmailAsync(ConfirmEmailAction action)
        {
            // TODO: добавить проверку на кол-во неверных попыток и блокировать временно подтверждение

            var user = await GetUserEntityAsync(action.UserId);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            if (!(user.Email == action.Email && user.ConfirmCode == action.Code))
                throw new InvalidateConfirmCodeApiException();
            
            user.ConfirmCode = "";
            user.ConfirmDate = new DateTime();
            user.EmailConfirmed = true;

            await _dbManager.SaveChangesAsync();

            return new ProfileResult(user);
        }

        public async Task<UserResult> VoteForUserAsync(UserVoteAction action)
        {
            var user = await GetUserEntityAsync(action.UserId);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            if (action.Vote < 1 || action.Vote > 5)
                throw new BadRequestApiException("Оценка должна быть в диапозоне от 1 до 5");

            var fromUser = await GetUserEntityAsync(action.VotedUserId);

            if (fromUser == null)
                throw new NotFoundApiException("Пользователь не найден");

            if (user == fromUser)
                throw new BadRequestApiException("ОНельза поставить оценку самому себе");

            if (user.Vote == null)
                user.Vote = new UserVoteEntity()
                {
                    User = user,
                    FromUser = fromUser,
                    Vote = action.Vote
                };
            else
                user.Vote.Vote = action.Vote;

            await _dbManager.SaveChangesAsync();

            return new UserResult(user);
        }

        public async Task AddDeviceAsync(AddDeviceAction action)
        {
            var user = await GetUserEntityAsync(action.UserId);

            if (user == null)
                throw new NotFoundApiException("Пользователь не найден");

            var device = new DeviceEntity()
            {
                DevicePlatform = action.DevicePlatform,
                FcmToken = action.FcmToken,
                User = user
            };

            await _dbManager.Devices.AddAsync(device);
            await _dbManager.SaveChangesAsync();
        }

        private async Task<UserEntity> GetUserEntityAsync(string id)
        {
            return await _dbManager.Users.Include(u => u.Vote).FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.DAL;
using Demo.Core.DAL.Entities;
using Demo.Core.Interfaces;
using Demo.Core.Models.Auth;
using Demo.Core.Settings;
using Demo.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Demo.Core.Helpers;
using Hangfire;

namespace Demo.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<UserEntity> _userManager;
        private readonly DbManager _dbManager;
        private readonly IMailService _mailService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public AuthService(IOptions<JwtSettings> jwtSettings, UserManager<UserEntity> userManager, DbManager dbManager, IMailService mailService, IBackgroundJobClient backgroundJobClient)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _dbManager = dbManager;
            _mailService = mailService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<AuthResult> LoginAsync(LoginAction action)
        {
            var user = await _userManager.FindByEmailAsync(action.Login);

            if (user != null && await _userManager.CheckPasswordAsync(user, action.Password))
            {
                if (user.RefreshTokens == null)
                    user.RefreshTokens = new List<RefreshTokenEntity>();

                var result = GetAuthResult(user);

                RefreshTokenEntity refreshToken = new()
                {
                    Token = result.RefreshToken,
                    User = user,
                    ExpiryOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays)
                };

                user.RefreshTokens.Add(refreshToken);

                await _dbManager.SaveChangesAsync();

                return result;
            }

            throw new UnauthorizedApiException();
        }

        public async Task<AuthResult> RegisterAsync(RegisterAction action)
        {
            var userExists = await _userManager.FindByEmailAsync(action.Email);
            if (userExists != null)
                throw new UserExistApiException();

            UserEntity user = new()
            {
                Email = action.Email,
                UserName = action.Email,
                Name = action.Name,
                PhoneNumber = action.Phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConfirmCode = StringHelper.GenerateRandomCode(),
                EmailConfirmed = false,
                RefreshTokens = new List<RefreshTokenEntity>()
            };

            var result = GetAuthResult(user);

            RefreshTokenEntity refreshToken = new()
            {
                Token = result.RefreshToken,
                User = user,
                ExpiryOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays)
            };

            user.RefreshTokens.Add(refreshToken);

            var userCreateTask = await _userManager.CreateAsync(user, action.Password);

            if (!userCreateTask.Succeeded)
                throw new BadRequestApiException("Ошибка создания пользователя. Повторите регистрацию позже.");

            _backgroundJobClient.Enqueue(() => _mailService.SendConfirmCode(user.Email, user.ConfirmCode));
            
            return result;
        }

        public async Task<AuthResult> RefreshTokenAsync(RefreshTokenAction action)
        {
            var user = await _dbManager
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == action.RefreshToken && rt.UserId == u.Id && rt.ExpiryOn > new DateTime()));

            if (user == null)
                throw new InvalidateRefreshTokenApiException();

            var result = GetAuthResult(user);

            RefreshTokenEntity refreshToken = new()
            {
                Token = result.RefreshToken,
                User = user,
                ExpiryOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays)
            };

            user.RefreshTokens.Add(refreshToken);

            await _dbManager.SaveChangesAsync();

            return result;
        }

        public async Task RestorePasswordAsync(RestorePasswordAction action)
        {
            var user = await _dbManager.Users.FirstOrDefaultAsync(u => u.Email == action.Email);

            if (user == null)
                throw new NotFoundApiException("Пользователь с таким email не найден.");

            string password = StringHelper.GenerateRandomPassword();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
                throw new ChangePasswordApiException();

            _backgroundJobClient.Enqueue(() => _mailService.SendNewPassword(user.Email, password));
        }

        private AuthResult GetAuthResult(UserEntity user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpiryTimeInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            string refreshToken = Convert.ToBase64String(randomNumber);

            return new AuthResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            };
        }
    }
}

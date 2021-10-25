using System;
using System.Threading.Tasks;
using Demo.Core.Models.Auth;

namespace Demo.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginAction action);
        Task<AuthResult> RegisterAsync(RegisterAction action);
        Task<AuthResult> RefreshTokenAsync(RefreshTokenAction action);
        Task RestorePasswordAsync(RestorePasswordAction action);
    }
}

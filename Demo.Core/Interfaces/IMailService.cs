using Demo.Core.DAL.Entities;

namespace Demo.Core.Interfaces
{
    public interface IMailService
    {
        void Send(string to, string subject, string body);
        void SendConfirmCode(string email, string confirmCode);
        void SendNewPassword(string email, string Password);
    }
}

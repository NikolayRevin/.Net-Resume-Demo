using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Demo.Core.DAL.Entities;
using Demo.Core.Interfaces;
using Demo.Core.Settings;
using Microsoft.Extensions.Options;

namespace Demo.Core.Services
{
    public class MailService: IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly SmtpClient _client = new();

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;

            _client = new();
            _client.Host = _mailSettings.Host;
            _client.Port = _mailSettings.Port;
            _client.EnableSsl = true;
            _client.Credentials = new NetworkCredential(_mailSettings.Login, _mailSettings.Password);   
        }

        public void Send(string to, string subject, string body)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_mailSettings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(to));

            string userToken = "";
            _client.SendAsync(mail, userToken);
        }

        public void SendConfirmCode(string email, string confirmCode)
        {
            string subject = "Demo: Подтверждение email";
            string body = "Здравствуйте.<br>" +
                "Вам пришло это письмо, поскольку вам необходимо подтвердить ваш email в приложении.<br>" +
                "Код подтверждения: <b>" + confirmCode + "</b>.<br>" +
                "Если вы не регистрировались, просто не реагируйте на это письмо.<br>" +
                "Письмо сгенерировано автоматически.";
            Send(email, subject, body);
        }

        public void SendNewPassword(string email, string password)
        {
            string subject = "Demo: Ваш новый пароль";
            string body = "Здравствуйте.<br>" +
                "Вам пришло это письмо, поскольку мы получили запрос на сброс пароля для вашего аккаунта в мобильном приложении BudgeUp.<br>" +
                "Ваш новый пароль: <b>" + password + "</b>.<br>" +
                "Письмо сгенерировано автоматически.";
            Send(email, subject, body);
        }
    }
}

using System;
namespace Demo.Core.Settings
{
    public class MailSettings
    {
        public const string SectionName = "MailSettings";

        public string From { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}

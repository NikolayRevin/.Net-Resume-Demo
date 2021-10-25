using System;

namespace Demo.Core.Settings
{
    public class CommonSettings
    {
        public const string SectionName = "CommonSettings";

        public string SqlConnectionString { get; set; }

        public bool ShowSwagger { get; set; }

        public bool ShowHangfileDashboard { get; set; }
    }
}

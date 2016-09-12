namespace Nlavri.Templifier
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AppConfiguration
    {
        public static IList<string> GetDirectoryExclusions()
        {
            return ParseList(GetAppSetting("DirectoryExclusions"));
        }

        public static IList<string> GetFileExclusions()
        {
            return ParseList(GetAppSetting("FileExclusions"));
        }

        public static IList<string> GetTokeniseFileExclusions()
        {
            return ParseList(GetAppSetting("TokeniseFileExclusions"));
        }

        private static string GetAppSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }

        private static IList<string> ParseList(string commaSeparatedString)
        {
            return string.IsNullOrEmpty(commaSeparatedString) ? new List<string>() : commaSeparatedString.Split(";".ToCharArray()).ToList();
        }
    }
}
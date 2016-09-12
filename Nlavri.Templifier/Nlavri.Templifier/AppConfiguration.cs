namespace Nlavri.Templifier
{
    using System.Configuration;

    #region Using Directives

    

    #endregion

    public class AppConfiguration
    {
        public string GetDirectoryExclusions()
        {
            return this.GetAppSetting("DirectoryExclusions");
        }

        public string GetFileExclusions()
        {
            return this.GetAppSetting("FileExclusions");
        }

        public string GetTokeniseFileExclusions()
        {
            return this.GetAppSetting("TokeniseFileExclusions");
        }

        private string GetAppSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName] ?? string.Empty;
        }
    }
}
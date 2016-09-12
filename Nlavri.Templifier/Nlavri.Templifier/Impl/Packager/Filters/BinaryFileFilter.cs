namespace Nlavri.Templifier.Impl.Packager.Filters
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Interfaces.Packager.Filters;

    #endregion

    public class BinaryFileFilter : IBinaryFileFilter
    {
        private readonly AppConfiguration configuration;
        private List<string> fileExclusions = new List<string>();

        public BinaryFileFilter(AppConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<string> Filter(IEnumerable<string> files)
        {
            this.SetFilters();
            return files.Where(file => !this.fileExclusions.Contains(new FileInfo(file).Extension.ToLowerInvariant())).ToList();
        }

        private void SetFilters()
        {
            this.fileExclusions = this.ParseList(this.configuration.GetTokeniseFileExclusions());
        }

        private List<string> ParseList(string commaSeparatedString)
        {
            return commaSeparatedString.Split(";".ToCharArray()).ToList();
        }
    }
}
namespace Nlavri.Templifier.Core
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Processors;
    using Tokeniser;

    #endregion

    public class PackageDeployer
    {
        #region Fields

        private readonly TemplateTokeniser templateTokeniser;

        #endregion

        public PackageDeployer(TemplateTokeniser templateTokeniser)
        {
            this.templateTokeniser = templateTokeniser;
        }

        public void DeployPackage(CommandOptions options)
        {
            ZipFile.ExtractToDirectory(options.PackagePath, options.Folder);

            this.ProcessFiles(options.Folder, options.Tokens);
            this.CleanUp(options.Folder, options.Tokens);
        }

        private void CleanUp(string path, Dictionary<string, string> tokens)
        {
            var directories = IoHelper.GetDirectories(path).ToList();

            foreach (var directory in directories)
            {
                foreach (var token in tokens)
                {
                    if (directory.Contains(token.Key))
                    {
                        IoHelper.DeleteDirectory(directory);
                    }
                }
            }
        }

        private void ProcessFiles(string path, Dictionary<string, string> tokens)
        {
            var files = IoHelper.GetFiles(path);

            this.ProcessFileContents(files, tokens);
            this.ProcessDirectoryAndFilePaths(files, tokens);
        }

        private void ProcessDirectoryAndFilePaths(IEnumerable<string> files, Dictionary<string, string> tokens)
        {
            foreach (var file in files)
            {
                this.templateTokeniser.TokeniseDirectoryAndFilePaths(file, tokens);
            }
        }

        private void ProcessFileContents(IEnumerable<string> files, Dictionary<string, string> tokens)
        {
            var filteredFiles =
                files.Where(
                    file =>
                        !AppConfiguration.GetTokeniseFileExclusions()
                            .Contains((Path.GetExtension(file) ?? string.Empty).ToLowerInvariant())).ToList();

            foreach (var file in filteredFiles)
            {
                this.templateTokeniser.TokeniseFileContent(file, tokens);
            }
        }
    }
}
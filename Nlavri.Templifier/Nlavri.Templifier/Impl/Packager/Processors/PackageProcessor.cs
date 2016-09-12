namespace Nlavri.Templifier.Impl.Packager.Processors
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Packager.Filters;
    using Interfaces.Packager.Processors;
    using Interfaces.Packager.Tokeniser;

    #endregion

    public class PackageProcessor : IPackageProcessor
    {
        #region Fields

        private readonly IArtefactProcessor artefactProcessor;
        private readonly IBinaryFileFilter binaryFileFilter;
        private readonly ICleanUpProcessor cleanUpProcessor;
        private readonly ITemplateTokeniser templateTokeniser;

        #endregion

        public PackageProcessor(
            IArtefactProcessor artefactProcessor,
            ICleanUpProcessor cleanUpProcessor,
            ITemplateTokeniser templateTokeniser,
            IBinaryFileFilter binaryFileFilter)
        {
            this.artefactProcessor = artefactProcessor;
            this.binaryFileFilter = binaryFileFilter;
            this.cleanUpProcessor = cleanUpProcessor;
            this.templateTokeniser = templateTokeniser;
        }

        public void Process(string path, Dictionary<string, string> tokens)
        {
            this.ProcessFiles(path, tokens);
            this.ProcessDirectories(path, tokens);
        }

        private void ProcessDirectories(string path, Dictionary<string, string> tokens)
        {
            var directories = this.artefactProcessor.RetrieveDirectories(path).ToList();

            foreach (var directory in directories)
            {
                foreach (var token in tokens)
                {
                    if (directory.Contains(token.Key))
                    {
                        this.cleanUpProcessor.Process(directory);
                    }
                }
            }
        }

        private void ProcessFiles(string path, Dictionary<string, string> tokens)
        {
            var files = this.artefactProcessor.RetrieveFiles(path);

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
            var filteredFiles = this.binaryFileFilter.Filter(files);

            foreach (var file in filteredFiles)
            {
                this.templateTokeniser.TokeniseFileContent(file, tokens);
            }
        }
    }
}
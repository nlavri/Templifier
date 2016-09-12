namespace Nlavri.Templifier.Impl.Packager.Builders
{
    #region Using Directives

    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Interfaces.Packager.Filters;
    using Interfaces.Packager.Processors;
    using Packages;

    #endregion

    public class TokenisedPackageBuilder
    {
        #region Fields

        private readonly IBinaryFileFilter binaryFileFilter;
        private readonly ManifestBuilder manifestBuilder;
        private readonly IFileContentProcessor fileContentProcessor;
        private readonly IRenameFileProcessor renameFileProcessor;

        #endregion

        public TokenisedPackageBuilder(
            ManifestBuilder manifestBuilder,
            IFileContentProcessor fileContentProcessor,
            IRenameFileProcessor renameFileProcessor,
            IBinaryFileFilter binaryFileFilter)
        {
            this.manifestBuilder = manifestBuilder;
            this.fileContentProcessor = fileContentProcessor;
            this.binaryFileFilter = binaryFileFilter;
            this.renameFileProcessor = renameFileProcessor;
        }

        public Package Tokenise(Package package, Dictionary<string, string> tokens)
        {
            var templatePath = Path.Combine(Path.Combine(Path.GetTempPath(), package.Manifest.Id.ToString()), "Template");
            if (!Directory.Exists(templatePath))
            {
                Directory.CreateDirectory(templatePath);
            }

            var result = new Package
            {
                Manifest = this.manifestBuilder.Build(package.Manifest.Name, templatePath, package.Manifest.Tokens),
                Path = templatePath
            };
            this.TokeniseDirectoriesAndFiles(package, result, tokens);
            this.TokeniseFileContent(result, tokens);

            return result;
        }

        private static string Replace(Dictionary<string, string> tokens, string value)
        {
            string result = value;

            foreach (var token in tokens)
            {
                result = Regex.Replace(result, token.Key, match => token.Value, RegexOptions.IgnoreCase);
            }

            return result;
        }

        private void TokeniseFileContent(Package package, Dictionary<string, string> tokens)
        {
            var processableFiles = this.binaryFileFilter.Filter(package.Manifest.Files);

            Parallel.ForEach(
                processableFiles,
                file =>
                    {
                        var contents = this.fileContentProcessor.ReadContents(file);
                        contents = Replace(tokens, contents);
                        this.fileContentProcessor.WriteContents(file, contents);
                    });
        }

        private void TokeniseDirectoriesAndFiles(Package package, Package destinationPackage, Dictionary<string, string> tokens)
        {
            var manifestFiles = new BlockingCollection<string>();

            Parallel.ForEach(
                package.Manifest.Files,
                file =>
                {
                    var tokenisedName = Replace(tokens, file);
                    tokenisedName = this.RebaseToTemplatePath(package, tokenisedName, destinationPackage.Path);
                    this.renameFileProcessor.Process(file, tokenisedName);
                    manifestFiles.Add(tokenisedName);
                });
            destinationPackage.Manifest.Files = manifestFiles.ToList();
        }

        private string RebaseToTemplatePath(Package package, string tokenisedName, string templatePath)
        {
            return tokenisedName.Replace(package.Path, templatePath);
        }
    }
}
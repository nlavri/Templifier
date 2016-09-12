namespace Nlavri.Templifier.Core
{
    #region Using Directives

    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Builders;
    using Processors;

    #endregion

    public class PackageCreator
    {
        #region Fields

        private readonly ClonePackageBuilder clonePackageBuilder;
        private readonly ManifestBuilder manifestBuilder;
        private readonly TokenisedPackageBuilder packageTokeniser;


        #endregion

        public PackageCreator(
            ClonePackageBuilder clonePackageBuilder,
            TokenisedPackageBuilder packageTokeniser,
            ManifestBuilder manifestBuilder)
        {
            this.clonePackageBuilder = clonePackageBuilder;
            this.packageTokeniser = packageTokeniser;
            this.manifestBuilder = manifestBuilder;
        }

        public void CreatePackage(CommandOptions options)
        {
            var package = new Package
            {
                Path = options.Folder,
                Manifest =
                    this.manifestBuilder.Build(Path.GetFileNameWithoutExtension(options.PackagePath), options.Folder,
                        options.Tokens.Select(kvp => kvp.Value).ToList())
            };

            var clonedPackage = this.clonePackageBuilder.Build(package);
            var tokenizedPackage = this.packageTokeniser.Tokenise(clonedPackage, options.Tokens);

            var resultFile = string.IsNullOrEmpty(Path.GetExtension(options.PackagePath))
                ? options.PackagePath + ".pkg"
                : options.PackagePath;
            ZipFile.CreateFromDirectory(tokenizedPackage.Path, resultFile, CompressionLevel.Optimal, false);

            IoHelper.DeleteDirectory(clonedPackage.Path);
            IoHelper.DeleteDirectory(tokenizedPackage.Path);
        }
    }
}
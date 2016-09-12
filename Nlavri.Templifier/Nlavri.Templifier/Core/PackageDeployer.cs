namespace Nlavri.Templifier.Core
{
    #region Using Directives

    using System.IO.Compression;
    using Interfaces.Packager.Processors;

    #endregion

    public class PackageDeployer
    {
        #region Fields

        private readonly IPackageProcessor packageProcessor;

        #endregion

        public PackageDeployer(IPackageProcessor packageProcessor)
        {
            this.packageProcessor = packageProcessor;
        }

        public void DeployPackage(CommandOptions options)
        {
            ZipFile.ExtractToDirectory(options.PackagePath, options.Folder);

            //Manifest manifest;
            //using (var manifestXmlStream = File.Open(Path.Combine(options.Folder, "manifest.xml"),FileMode.Open))
            //{
            //    var serializer = new XmlSerializer(typeof(Manifest));
            //    manifest = (Manifest)serializer.Deserialize(manifestXmlStream);
            //}

            this.packageProcessor.Process(options.Folder, options.Tokens);
        }
    }
}
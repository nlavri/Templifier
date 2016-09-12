namespace Nlavri.Templifier.Impl.Packager.Builders
{
    #region Using Directives

    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading.Tasks;
    using Interfaces.Packager.Processors;
    using Newtonsoft.Json;
    using Packages;

    #endregion

    public class ClonePackageBuilder
    {
        private readonly ICloneFileProcessor cloneFileProcessor;
        private readonly ManifestBuilder manifestBuilder;

        public ClonePackageBuilder(ICloneFileProcessor cloneFileProcessor, ManifestBuilder manifestBuilder)
        {
            this.cloneFileProcessor = cloneFileProcessor;
            this.manifestBuilder = manifestBuilder;
        }

        public Package Build(Package package)
        {
            var newPath = Path.Combine(Path.Combine(Path.GetTempPath(), package.Manifest.Id.ToString()), "Cloned");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var result = new Package
            {
                Manifest = this.manifestBuilder.Build(package.Manifest.Name, newPath, package.Manifest.Tokens),
                Path = newPath
            };

            var files = new BlockingCollection<string>();

            Parallel.ForEach(
                package.Manifest.Files,
                file =>
                    {
                        var clonedPath = Path.Combine(result.Path, file);

                        this.cloneFileProcessor.Process(Path.Combine(package.Path, file), clonedPath);

                        files.Add(clonedPath);
                    });
            foreach (var manifestFile in files)
            {
                result.Manifest.Files.Add(manifestFile);
            }

            var manifestFilePath = this.PersistManifestFileAndReturnLocation(result);
            // Add the manifest file so that it will be tokenised.
            result.Manifest.Files.Add(manifestFilePath);

            return result;
        }

        private string PersistManifestFileAndReturnLocation(Package package)
        {
            var manifestFilePath = Path.Combine(package.Path, "manifest.json");

            using (var file = File.CreateText(manifestFilePath))
            {
                file.Write(JsonConvert.SerializeObject(package));
            }

            return manifestFilePath;
        }
    }
}
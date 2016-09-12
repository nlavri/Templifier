namespace Nlavri.Templifier.Impl.Packager.Builders
{
    #region Using Directives

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces.Packager.Processors;
    using Packages;

    #endregion

    public class ManifestBuilder 
    {
        private readonly IArtefactProcessor artefactProcessor;

        public ManifestBuilder(IArtefactProcessor artefactProcessor)
        {
            this.artefactProcessor = artefactProcessor;
        }

        public Manifest Build(string packageName, string sourcePath, IList<string> tokens)
        {
            var files = this.artefactProcessor.RetrieveFiles(sourcePath);

            var manifest = new Manifest
            {
                Id = Guid.NewGuid(),
                Name = packageName,
                Tokens = tokens,
            };

            var manifestFiles = new BlockingCollection<string>();

            Parallel.ForEach(files, file =>
            {
                manifestFiles.Add(StripParentPath(sourcePath, file));
            });

            manifest.Files = manifestFiles.ToList();

            return manifest;
        }

        private static string StripParentPath(string parentPath, string filePath)
        {
            return filePath.Replace(string.Concat(parentPath, "\\"), string.Empty);
        }
    }
}
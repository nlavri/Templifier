namespace Nlavri.Templifier.Impl.Packager.Processors
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Interfaces.Packager.Processors;

    #endregion

    public class FilteredFileSystemArtefactProcessor : IArtefactProcessor
    {
        private readonly AppConfiguration appConfiguration;

        public FilteredFileSystemArtefactProcessor(AppConfiguration appConfiguration)
        {
            this.appConfiguration = appConfiguration;
        }

        public IEnumerable<string> RetrieveFiles(string path)
        {
            return Flatten(path, Directory.GetDirectories).SelectMany(Directory.EnumerateFiles);
        }

        public void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public virtual IEnumerable<string> RetrieveDirectories(string path)
        {
            return Flatten(path, Directory.EnumerateDirectories);
        }

        private static IEnumerable<T> Flatten<T>(T item, Func<T, IEnumerable<T>> next)
        {
            yield return item;

            foreach (T flattenedChild in next(item).SelectMany(child => Flatten(child, next)))
            {
                yield return flattenedChild;
            }
        }

        private bool ShouldExclude(string path)
        {
            var fileExclusions = this.ParseList(this.appConfiguration.GetFileExclusions());
            var directoryExclusions = this.ParseList(this.appConfiguration.GetDirectoryExclusions());

            var segments = path.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            var shouldExclude = segments.Any(directory => directoryExclusions.Any(exclusion => Regex.IsMatch(directory, exclusion)));

            if (!shouldExclude)
            {
                var file = segments.Last();
                shouldExclude = fileExclusions.Any(exclusion => Regex.IsMatch(file, exclusion));
            }

            return shouldExclude;
        }

        private List<string> ParseList(string commaSeparatedString)
        {
            return commaSeparatedString.Split(";".ToCharArray()).ToList();
        }
    }
}
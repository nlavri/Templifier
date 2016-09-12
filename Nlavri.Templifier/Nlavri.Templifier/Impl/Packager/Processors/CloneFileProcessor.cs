namespace Nlavri.Templifier.Impl.Packager.Processors
{
    #region Using Directives

    using System.IO;
    using Interfaces.Packager.Processors;

    #endregion

    public class CloneFileProcessor : ICloneFileProcessor
    {
        public void Process(string sourcePath, string destinationPath)
        {
            var file = new FileInfo(destinationPath);

            if (file.Directory != null && !file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.Copy(sourcePath, destinationPath);
        }
    }
}
namespace Nlavri.Templifier.Impl.Packager.Processors
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using Interfaces.Packager.Processors;

    #endregion

    public class CleanUpProcessor : ICleanUpProcessor
    {
        public void Process(string path)
        {
            if (Directory.Exists(path))
            {
                ForceDeleteDirectory(path);
            }
        }

        private static void ForceDeleteDirectory(string path)
        {
            var folders = new Stack<DirectoryInfo>();
            var root = new DirectoryInfo(path);
            folders.Push(root);

            while (folders.Count > 0)
            {
                DirectoryInfo currentFolder = folders.Pop();
                currentFolder.Attributes = currentFolder.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);

                foreach (var d in currentFolder.GetDirectories())
                {
                    folders.Push(d);
                }

                foreach (var fileInFolder in currentFolder.GetFiles())
                {
                    fileInFolder.Attributes = fileInFolder.Attributes & ~(FileAttributes.Archive | FileAttributes.ReadOnly | FileAttributes.Hidden);
                    fileInFolder.Delete();
                }
            }

            root.Delete(true);
        }
    }
}
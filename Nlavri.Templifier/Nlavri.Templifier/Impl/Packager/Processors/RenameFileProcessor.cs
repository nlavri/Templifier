namespace Nlavri.Templifier.Impl.Packager.Processors
{
    #region Using Directives

    using System.IO;
    using Interfaces.Packager.Processors;

    #endregion

    public class RenameFileProcessor : IRenameFileProcessor
    {
        public void Process(string oldName, string newName)
        {
            var file = new FileInfo(newName);

            if (file.Directory != null && !file.Directory.Exists)
            {
                file.Directory.Create();
            }

            File.Move(oldName, newName);
        }
    }
}
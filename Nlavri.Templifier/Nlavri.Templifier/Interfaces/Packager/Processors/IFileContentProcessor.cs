namespace Nlavri.Templifier.Interfaces.Packager.Processors
{
    public interface IFileContentProcessor
    {
        string ReadContents(string path);

        void WriteContents(string path, string content);
    }
}
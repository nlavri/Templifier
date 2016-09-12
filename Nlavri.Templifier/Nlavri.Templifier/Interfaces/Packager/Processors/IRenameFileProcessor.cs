namespace Nlavri.Templifier.Interfaces.Packager.Processors
{
    public interface IRenameFileProcessor
    {
        void Process(string oldName, string newName);
    }
}
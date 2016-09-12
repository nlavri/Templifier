namespace Nlavri.Templifier.Interfaces.Packager.Processors
{
    using System.Collections.Generic;

    public interface IPackageProcessor
    {
        void Process(string path, Dictionary<string, string> tokens);
    }
}
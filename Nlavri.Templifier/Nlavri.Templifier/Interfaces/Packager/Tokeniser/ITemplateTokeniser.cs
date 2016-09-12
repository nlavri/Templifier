namespace Nlavri.Templifier.Interfaces.Packager.Tokeniser
{
    using System.Collections.Generic;

    public interface ITemplateTokeniser
    {
        void TokeniseFileContent(string file, Dictionary<string, string> tokens);

        void TokeniseDirectoryAndFilePaths(string file, Dictionary<string, string> tokens);
    }
}
namespace Nlavri.Templifier.Impl.Packager.Tokeniser
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Interfaces.Packager.Processors;
    using Interfaces.Packager.Tokeniser;

    #endregion

    public class TemplateTokeniser : ITemplateTokeniser
    {
        private readonly IFileContentProcessor fileContentProcessor;
        private readonly IRenameFileProcessor renameFileProcessor;

        public TemplateTokeniser(IRenameFileProcessor renameFileProcessor, IFileContentProcessor fileContentProcessor)
        {
            this.renameFileProcessor = renameFileProcessor;
            this.fileContentProcessor = fileContentProcessor;
        }

        public void TokeniseDirectoryAndFilePaths(string file, Dictionary<string, string> tokens)
        {
            var tokenisedName = Replace(tokens, file);
            this.renameFileProcessor.Process(file, tokenisedName);
        }

        public void TokeniseFileContent(string file, Dictionary<string, string> tokens)
        {
            var contents = this.fileContentProcessor.ReadContents(file);
            contents = Replace(tokens, contents);
            this.fileContentProcessor.WriteContents(file, contents);
        }

        private static string Replace(Dictionary<string, string> tokens, string value)
        {
            return tokens.Aggregate(value, (current, token) => Regex.Replace(current, token.Key, match => token.Value));
        }
    }
}
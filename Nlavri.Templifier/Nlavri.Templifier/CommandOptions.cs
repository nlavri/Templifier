namespace Nlavri.Templifier
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandLine;
    using CommandLine.Text;

    #endregion

    public class CommandOptions
    {
        private string rawMode;
        private string[] rawTokens;

        public CommandOptions()
        {
            this.SelectedMode = Mode.NotSet;
            this.Tokens = new Dictionary<string, string>();
        }

        public Mode SelectedMode { get; private set; }

        [Option('f', "folder", Required = true, HelpText = "Source path to be used when creating a package, or the destination path when deploying.")]
        public string Folder { get; set; }

        [Option('p', "package", Required = true, HelpText = @"Package to be created, or package to be deployed.")]
        public string PackagePath { get; set; }

        [Option('m', "mode", Required = true, HelpText = "Specifies whether to (c)reate/(d)eploy a package")]
        public string RawMode
        {
            get
            {
                return this.rawMode;
            }

            set
            {
                this.rawMode = value;

                switch (this.rawMode.ToLowerInvariant())
                {
                    case "c":
                        this.SelectedMode = Mode.Create;
                        break;
                    case "d":
                        this.SelectedMode = Mode.Deploy;
                        break;
                }
            }
        }

        [OptionArray('t', "tokens", HelpText = "Specifies name/value pairs to be used as part of token replacement.")]
        public string[] RawTokens
        {
            get
            {
                return this.rawTokens;
            }

            set
            {
                this.rawTokens = value;

                var tokens = this.rawTokens.Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Split('='));

                foreach (var token in tokens)
                {
                    if (token.Length == 2)
                    {
                        if (string.IsNullOrEmpty(token[1]))
                        {
                            throw new ArgumentException("Token is Malformed");
                        }

                        if (this.Tokens.ContainsKey(token[0]))
                        {
                            this.Tokens[token[0]] = token[1];
                        }
                        else
                        {
                            this.Tokens.Add(token[0], token[1]);
                        }

                    }
                }
            }
        }

        public Dictionary<string, string> Tokens { get; private set; }

        [HelpOption(HelpText = "Display this help text.")]
        public string GetUsage()
        {
            var help = new HelpText(new HeadingInfo("Templify",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()))
            {
                AdditionalNewLineAfterOption = false,
                MaximumDisplayWidth = Console.WindowWidth,
                Copyright = new CopyrightInfo("nlavri", 2016)
            };

            help.AddPreOptionsLine("Usage:");
            help.AddPreOptionsLine("    TemplifyCmd.exe -m c -f C:\\MySolution -p C:\\MyTemplate.pkg -t \"MySolution=__NAME__\"");
            help.AddPreOptionsLine("    TemplifyCmd.exe -m d -f C:\\MySolution -p C:\\MyTemplate.pkg -t \"__NAME__=MyNewSolution\" ");

            help.AddOptions(this);

            return help;
        }
        
        public enum Mode
        {
            NotSet,
            Create,
            Deploy,
        }
    }
}
using System;

namespace Nlavri.Templifier
{
    using Core;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new CommandOptions();
                CommandLine.Parser.Default.ParseArguments(args, options);

                var container = IoC.Init();

                switch (options.SelectedMode)
                {
                    case CommandOptions.Mode.Create:
                        container.GetInstance<PackageCreator>().CreatePackage(options);
                        break;
                    case CommandOptions.Mode.Deploy:
                        container.GetInstance<PackageDeployer>().DeployPackage(options);
                        break;
                    default:
                        Console.WriteLine(options.GetUsage());
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Templify encountered an error: ");
                Console.WriteLine(exception.Message);
            }
        }
    }
}

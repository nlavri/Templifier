namespace Nlavri.Templifier
{
    using Core;
    using Core.Builders;
    using Core.Processors;
    using Core.Tokeniser;
    using SimpleInjector;

    public static class IoC
    {
        public static Container Init()
        {
            var container = new Container();
            container.Options.AllowOverridingRegistrations = true;

            container.RegisterSingleton<PackageCreator>();
            container.RegisterSingleton<PackageDeployer>();
            container.RegisterSingleton<TemplateTokeniser>();
            container.RegisterSingleton<ManifestBuilder>();
            container.RegisterSingleton<TokenisedPackageBuilder>();
            container.RegisterSingleton<ClonePackageBuilder>();
            container.RegisterSingleton<FileContentProcessor>();
            
            return container;
        }
    }
}

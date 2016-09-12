namespace Nlavri.Templifier
{
    using Core;
    using Impl.Packager.Builders;
    using Impl.Packager.Filters;
    using Impl.Packager.Processors;
    using Impl.Packager.Tokeniser;
    using Interfaces.Packager.Filters;
    using Interfaces.Packager.Processors;
    using Interfaces.Packager.Tokeniser;
    using SimpleInjector;

    public static class IoC
    {
        public static Container Init()
        {
            var container = new Container();
            container.Options.AllowOverridingRegistrations = true;

            container.RegisterSingleton<AppConfiguration>();
            container.RegisterSingleton<PackageCreator>();
            container.RegisterSingleton<PackageDeployer>();
            container.RegisterSingleton<ITemplateTokeniser, TemplateTokeniser>();
            container.RegisterSingleton<IPackageProcessor, PackageProcessor>();
            container.RegisterSingleton<IArtefactProcessor, FilteredFileSystemArtefactProcessor>();
            container.RegisterSingleton<ManifestBuilder>();
            container.RegisterSingleton<TokenisedPackageBuilder>();
            container.RegisterSingleton<ClonePackageBuilder>();
            container.RegisterSingleton<ICloneFileProcessor, CloneFileProcessor>();
            container.RegisterSingleton<ICleanUpProcessor, CleanUpProcessor>();
            container.RegisterSingleton<IFileContentProcessor, FileContentProcessor>();
            container.RegisterSingleton<IRenameFileProcessor, RenameFileProcessor>();
            container.RegisterSingleton<IBinaryFileFilter, BinaryFileFilter>();

            return container;
        }
    }
}

namespace Nlavri.Templifier.Interfaces.Packager.Filters
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public interface IBinaryFileFilter
    {
        IEnumerable<string> Filter(IEnumerable<string> files);
    }
}
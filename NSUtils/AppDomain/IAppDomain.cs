using System.Collections.Generic;

namespace NSUtils.AppDomain
{
    public interface IAppDomain
    {
        IList<IAssembly> GetAssemblies();
    }
}

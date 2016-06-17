using System;

namespace NSUtils.AppDomain
{
    public interface IAssembly
    {
        string GetName { get; }
        Type[] GetTypes { get; }
    }
}

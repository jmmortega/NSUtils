
using System.Windows.Input;

namespace NSUtilsMvx.ExtensionMethods
{
    public static class ExtensionMethodsMvx
    {
        public static void Execute(this ICommand command)
        {
            command.Execute(null);
        }
    }
}

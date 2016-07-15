
using System.Windows.Input;

namespace NSUtils
{
    public static class ExtensionMethodsMvx
    {
        public static void Execute(this ICommand command)
        {
            command.Execute(null);
        }
    }
}

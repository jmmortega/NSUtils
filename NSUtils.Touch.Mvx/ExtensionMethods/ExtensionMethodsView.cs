using MvvmCross.Platform.UI;
using UIKit;

namespace NSUtils.Touch.Mvx.ExtensionMethods
{
    public static class ExtensionMethodsView
    {
        public static UIColor ToAndroidColor(this MvxColor color)
        {
            return new UIColor(color.R, color.G, color.B, color.A);
        }
    }
}

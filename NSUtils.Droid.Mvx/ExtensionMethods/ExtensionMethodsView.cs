using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Platform.UI;

namespace NSUtils
{
    public static class ExtensionMethodsView
    {
        public static Color ToAndroidColor(this MvxColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
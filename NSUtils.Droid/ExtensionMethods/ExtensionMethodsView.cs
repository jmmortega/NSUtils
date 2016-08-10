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
using Android.Views.InputMethods;
using Android.Util;
using Android.Graphics.Drawables;
using Android;

namespace NSUtils
{
    public static class ExtensionMethodsView
    {
        public static List<View> GetChildrenViews(this View parentView)
        {
            List<View> childrenViews = new List<View>();

            if (parentView is ViewGroup)
            {
                var vg = (ViewGroup)parentView;

                for (int i = 0; i < vg.ChildCount; i++)
                {
                    var child = vg.GetChildAt(i);

                    childrenViews.Add(child);
                }
            }
            else
            {
                childrenViews.Add(parentView);
            }

            return childrenViews;
        }


        /// <summary>
        /// Get all the children views in this view, and the ViewGroup inside.        
        /// </summary>
        /// <param name="parentView"></param>
        /// <returns></returns>
        public static List<View> GetDeepChildrenViews(this View parentView)
        {
            List<View> childrenViews = new List<View>();

            if (parentView is ViewGroup)
            {
                var vg = (ViewGroup)parentView;

                for (int i = 0; i < vg.ChildCount; i++)
                {
                    var child = vg.GetChildAt(i);

                    if (child is ViewGroup)
                    {
                        childrenViews.AddRange(child.GetDeepChildrenViews());
                    }

                    childrenViews.Add(child);
                }
            }
            else
            {
                childrenViews.Add(parentView);
            }

            return childrenViews;
        }

        public static List<T> GetTypeView<T>(this List<View> views) where T : View
        {
            List<T> myTypeView = new List<T>();

            foreach (View view in views)
            {
                if (view is T)
                {
                    myTypeView.Add((T)view);
                }
            }

            return myTypeView;
        }

        public static T GetViewById<T>(this List<View> views, int id) where T : View
        {
            return (T)views.FirstOrDefault(x => x.Id == id);
        }

        public static void HideKeyBoard(Activity activity, IBinder windowToken)
        {
            var inputManager = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(windowToken, HideSoftInputFlags.None);
        }

        public static void ShowKeyBoard(Activity activity, View viewSelected)
        {
            var inputManager = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
            inputManager.ShowSoftInput(viewSelected, ShowFlags.Forced);
        }

        public static bool isPhone(this Activity activity)
        {
            var metrics = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(metrics);

            var widthInches = metrics.WidthPixels / metrics.Xdpi;
            var isPhone = widthInches < 5;
            return isPhone;            
        }

        public static bool CheckForeground(this Context context)
        {            
            ActivityManager activityManager = (ActivityManager)context.GetSystemService(Context.ActivityService);

            var appProcesses = activityManager.RunningAppProcesses;

            string packageName = context.PackageName;

            var process = appProcesses.FirstOrDefault(x => x.Importance == Importance.Foreground && x.ProcessName == packageName);

            return process != null;
        }

        public static bool KillProcess(this Context context, string processName)
        {            
            ActivityManager activityManager = (ActivityManager)context.GetSystemService(Context.ActivityService);

            var process = activityManager.RunningAppProcesses.FirstOrDefault(x => x.ProcessName == processName);

            if (process != null)
            {
                Android.OS.Process.KillProcess(process.Pid);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Drawable GetDrawableByName(this Context context, string name)
        {            
            if (!string.IsNullOrEmpty(name))
            {
                int resourceId = context.Resources.GetIdentifier(name, "drawable", context.PackageName);
                return context.Resources.GetDrawable(resourceId);
            }
            else
            {
                return null;
            }

        }
    }
}
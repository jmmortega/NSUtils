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
    }
}
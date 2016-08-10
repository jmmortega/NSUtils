using System;
using System.Collections.Generic;
using NSUtils;
using System.Linq;
using UIKit;
using System.Drawing;
using Foundation;

namespace NSUtils
{
    public static class ExtensionMethodsView
    {
        public static List<UIView> GetDeepViews(this UIView parentView)
        {
            List<UIView> views = new List<UIView>();

            foreach (UIView view in parentView.Subviews)
            {
                if (view.Subviews.Length > 0)
                {
                    views.AddRange(GetDeepViews(view));
                }
                views.Add(view);
            }

            return views;
        }

        public static List<T> GetChildrenViews<T>(this UIView rootView) where T : UIView
        {
            var views = GetDeepViews(rootView);            
            return views.Where(x => x.GetType() == typeof(T) || x.GetType().GetParentTypes().Contains(typeof(T))).Select(x => (T)x).ToList();
        }

        public static SizeF MeasureTextSize(this string text, double width, double fontsize, string fontName = "HelveticaNeue")
        {
            var nsText = new NSString(text);
            var boundSize = new SizeF((float)width, float.MaxValue);

            var options = NSStringDrawingOptions.UsesFontLeading | NSStringDrawingOptions.UsesLineFragmentOrigin;

            var attributes = new UIStringAttributes
            {
                Font = UIFont.FromName(fontName, (float)fontsize)
            };

            var size = nsText.GetBoundingRect(boundSize, options, attributes, null).Size;

            return new SizeF((float)size.Width, (float)size.Height);
        }
    }
}

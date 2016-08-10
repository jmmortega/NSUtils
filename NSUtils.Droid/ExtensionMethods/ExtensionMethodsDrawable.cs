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
using Android.Graphics.Drawables;

namespace NSUtils
{
    public static class ExtensionMethodsDrawable
    {
        public static Bitmap ToBitmap(this Drawable drawable)
        {
            Bitmap bitmap = null;
            try
            {
                bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);

                drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                drawable.Draw(canvas);

                return bitmap;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return bitmap;
        }

        public static Drawable ToDrawableFromBase64(this string base64Image)
        {
            byte[] image = System.Convert.FromBase64String(base64Image);
            BitmapDrawable drawable = null;
            drawable = new BitmapDrawable(BitmapFactory.DecodeByteArray(image, 0, (int)image.Length));

            return drawable;
        }

        public static Bitmap ToBitmapFromBase64(this string base64Image)
        {
            byte[] image = System.Convert.FromBase64String(base64Image);
            return BitmapFactory.DecodeByteArray(image, 0, (int)image.Length);
        }

        public static Drawable ChangeColor(this Drawable drawable, Color fromColor, Color targetColor, float tolerance = 20)
        {
            return new BitmapDrawable(ChangeColor(drawable.ToBitmap(), fromColor, targetColor));
        }

        private static Bitmap ChangeColor(Bitmap bitmap, Color fromColor, Color targetColor, float tolerance = 20)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int[] pixels = new int[width * height];

            float[] redRange = new float[2]{
                (float)Math.Max(fromColor.R - (tolerance / 2), 0.0),
                (float)Math.Min(fromColor.R + (tolerance / 2), 255.0)};

            float[] greenRange = new float[2]{
                (float)Math.Max(fromColor.G - (tolerance / 2), 0.0),
                (float)Math.Min(fromColor.G + (tolerance / 2), 255.0)};

            float[] blueRange = new float[2]{
                (float)Math.Max(fromColor.B - (tolerance / 2), 0.0),
                (float)Math.Min(fromColor.B + (tolerance / 2), 255.0)};

            bitmap.GetPixels(pixels, 0, width, 0, 0, width, height);

            for (int i = 0; i < pixels.Length; i++)
            {

                if (pixels[i] == fromColor)
                {
                    pixels[i] = new Color(targetColor.R, targetColor.G, targetColor.B, targetColor.A - 1);
                }

                int red = Color.GetRedComponent(pixels[i]);
                int green = Color.GetGreenComponent(pixels[i]);
                int blue = Color.GetBlueComponent(pixels[i]);
                int alpha = Color.GetAlphaComponent(pixels[i]);

                if (((red >= redRange[0]) && (red <= redRange[1])) &&
                    ((green >= greenRange[0]) && (green <= greenRange[1])) &&
                    ((blue >= blueRange[0]) && (blue <= blueRange[1])) &&
                    ((alpha > 0 && alpha < 254)))
                {
                    pixels[i] = new Color(targetColor.R, targetColor.G, targetColor.B, targetColor.A - 1);
                }
            }

            if (bitmap.IsMutable)
            {
                bitmap.SetPixels(pixels, 0, width, 0, 0, width, height);
                return bitmap;
            }
            else
            {
                var mutableBitmap = bitmap.Copy(bitmap.GetConfig(), true);
                mutableBitmap.SetPixels(pixels, 0, width, 0, 0, width, height);
                return mutableBitmap;
            }
        }
    }
}
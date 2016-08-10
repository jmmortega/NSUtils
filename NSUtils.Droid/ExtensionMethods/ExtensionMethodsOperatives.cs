using System;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Net;
using Android.Net.Wifi;
using Android.Telephony;
using NSUtils.Model;
using System.Drawing;

namespace NSUtils
{
    public static class ExtensionMethodsOperatives
    {
        public static bool OpenApp(this Context context, string packageName, string parameters = null)
        {
            PackageManager manager = context.PackageManager;

            try
            {
                Intent intent = manager.GetLaunchIntentForPackage(packageName);

                if (intent == null)
                {
                    return false;
                }

                intent.AddCategory(Intent.CategoryLauncher);

                if (!string.IsNullOrEmpty(parameters))
                {
                    intent.SetData(Android.Net.Uri.Parse(parameters));
                }

                context.StartActivity(intent);
                return true;
            }
            catch (Exception e)
            {                
                return false;
            }
        }

        public static void OpenWeb(this Context context, string url)
        {
            Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
            context.StartActivity(browserIntent);
        }

        public static bool CheckAppInstalled(this Context context, string packageName)
        {
            PackageManager packageManager = context.PackageManager;

            try
            {
                packageManager.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void InstallApp(this Context context, string packageName)
        {
            context.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse(string.Format("market://details?id={0}", packageName))));
        }

        public static float GetBatteryLevel(this Context context)
        {
            Intent batteryIntent = context.RegisterReceiver(null, new IntentFilter(Intent.ActionBatteryChanged));
            int level = batteryIntent.GetIntExtra(BatteryManager.ExtraLevel, -1);
            int scale = batteryIntent.GetIntExtra(BatteryManager.ExtraScale, -1);

            if (level == -1 || scale == -1)
            {
                return 50.0f;
            }

            return ((float)level / (float)scale) * 100.0f;
        }

        public static string GetNetworkLevel(this Context context)
        {
            try
            {                
                ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                var hasWifi = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);

                if (hasWifi != null)
                {
                    var wifiManager = (WifiManager)context.GetSystemService(Context.WifiService);
                    var wifiInfo = wifiManager.ConnectionInfo;

                    if (wifiInfo != null)
                    {
                        return string.Format("Wifi {0} mbps", wifiInfo.LinkSpeed);
                    }
                }

                var hasMobileNetwork = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile);

                if (hasMobileNetwork != null)
                {
                    var telManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);

                    var networkInfo = telManager.NetworkType;

                    return string.Format("Mobile network {0}", networkInfo);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return string.Empty;
        }

        public static DeviceInfo GetDeviceInfo(this Context context)
        {            
            var package = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            var androidVersionName = package.VersionName;

            return new DeviceInfo()
            {
                SDK = Android.OS.Build.VERSION.Sdk,
                Device = Android.OS.Build.Device,
                Model = Android.OS.Build.Model,
                Product = Android.OS.Build.Product,
                ApplicationVersion = androidVersionName,
                BatteryLevel = context.GetBatteryLevel(),
                Network = context.GetNetworkLevel()
            };
        }

        public static string GetVersionName(this Context context)
        {
            string versionName = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            return versionName;
        }

        public static Size GetSizeScreen(this Context context)
        {
            var displayMetrics = context.Resources.DisplayMetrics;
            return new Size(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
        }
    }
}
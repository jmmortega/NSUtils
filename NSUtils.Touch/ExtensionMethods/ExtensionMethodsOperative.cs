using Foundation;
using UIKit;

namespace NSUtils
{
    public static class ExtensionMethodsOperative
    {
        public static void OpenChrome(string url)
        {
            string schemaUrl = string.Empty;

            if (url.Contains("itms-services://"))
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            else
            {
                //Only one tab
                schemaUrl = string.Format("googlechrome-x-callback://x-callback-url/open/?url={0}", url.EncodeRFC3986());

                var nsSchemaUrl = new NSUrl(schemaUrl);

                if (UIApplication.SharedApplication.CanOpenUrl(nsSchemaUrl) == true)
                {
                    UIApplication.SharedApplication.OpenUrl(nsSchemaUrl);
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                }
            }
        }
    }
}

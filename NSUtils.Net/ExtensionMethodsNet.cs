using System.IO;

namespace NSUtils
{
    public static class ExtensionMethodsNet
    {
        public static string ToResponseString(this Stream stream)
        {
            if(stream != null)
            {
                return new StreamReader(stream).ReadToEnd();                
            }
            return string.Empty;
        }

        public static string ToResponseString(this Response response)
        {
            return response?.ResponseStream?.ToResponseString();            
        }
    }
}

using System.IO;
using System.Net;

namespace NSUtils.Net
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        public Stream ResponseStream { get; set; }

        public string ToResponseString()
        {
            if(ResponseStream != null)
            {
                return new StreamReader(ResponseStream).ReadToEnd();
            }
            return string.Empty;
        }
    }
}

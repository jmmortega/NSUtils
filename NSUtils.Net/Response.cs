using System.IO;
using System.Net;

namespace NSUtils
{
    public class Response 
    {
        public Stream ResponseStream { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}

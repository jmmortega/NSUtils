using System;
using System.IO;
using System.Net;

namespace NSUtils
{
    public class ResponseError : Response
    {
        public ResponseError(WebException e)
        {
            Message = e.Message;
            Exception = e;
            var httpResponse = (HttpWebResponse)e.Response;
            ResponseStream = e.Response.GetResponseStream();
            StatusCode = httpResponse.StatusCode;
        }

        public ResponseError(Exception e)
        {
            Message = e.Message;
            Exception = new WebException(e.Message);
            ResponseStream = null;
            StatusCode = HttpStatusCode.NotFound;
            IsWebException = true;
        }

        public string Message { get; set; }

        public WebException Exception { get; set; }

        public bool IsWebException { get; set; }
    }
}

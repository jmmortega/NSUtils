using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace NSUtils
{
    public class HttpCalls
    {
        public HttpCalls()
        {
            ContentType = "application/x-www-form-urlencoded";
            
        }
                
        private Dictionary<string, string> m_headers;

        public Dictionary<string, string> Headers
        {
            get
            {
                if (m_headers == null)
                {
                    m_headers = new Dictionary<string, string>();
                }
                return m_headers;
            }
            set { m_headers = value; }
        }

        public string ContentType { get; set; }

        #region Public Methods

        public void Get(string url, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {            
            var request = InstanceWebRequest(url);
            request.Method = "GET";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("GET Request to {0}", new object[] { url }));

            CallHttp(request, callbackOK, callbackError);
        }

        public Task<Response> GetAsync(string url)
        {
            var request = InstanceWebRequest(url);
            request.Method = "GET";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("GET Request to {0}", new object[] { url }));

            return CallHttpAsync(request);
        }

        public Task<Response> PostAsync(string url)
        {
            var request = InstanceWebRequest(url);
            request.Method = "POST";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("POST Request to {0}", new object[] { url }));

            return CallHttpAsync(request);
        }
        
        public void Post(string url, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            var request = InstanceWebRequest(url);
            request.ContentType = ContentType;
            request.Method = "POST";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("POST Request to {0}", new object[] { url }));

            CallHttp(request, callbackOK, callbackError);
        }

        public Task<Response> PostAsync(string url, string body)
        {
            byte[] data = Encoding.UTF8.GetBytes(body);

            return this.PostAsync(url, data);
        }

        public void Post(string url, string body, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            byte[] data = Encoding.UTF8.GetBytes(body);

            this.Post(url, data, callbackOK, callbackError);
        }

        public void Delete(string url, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            var request = InstanceWebRequest(url);
            request.ContentType = ContentType;
            request.Method = "DELETE";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine($"DELETE Request to {url}");

            CallHttp(request, callbackOK, callbackError);
        }

        public Task<Response> DeleteAsync(string url)
        {
            var request = InstanceWebRequest(url);
            request.Method = "DELETE";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine($"DELETE Request to {url}");
            return CallHttpAsync(request);
        }

        public Task<Response> DeleteAsync(string url , byte[] body)
        {
            var request = InstanceWebRequest(url);
            request.Method = "DELETE";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("DELETE Request to {0}", new object[] { url }));
            return CallHttpWithBodyAsync(request, body);
        }

        public void Delete(string url, byte[] body, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            var request = InstanceWebRequest(url);
            request.Method = "DELETE";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("DELETE Request to {0}", new object[] { url }));
            CallHttpWithBody(request, body, callbackOK, callbackError);
        }

        public Task<Response> PutAsync(string url, byte[] body)
        {
            var request = InstanceWebRequest(url);
            request.Method = "PUT";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("PUT Request to {0}", new object[] { url }));
            return CallHttpWithBodyAsync(request, body);
        }

        public void Put(string url, byte[] body, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            var request = InstanceWebRequest(url);
            request.Method = "PUT";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("PUT Request to {0}", new object[] { url }));
            CallHttpWithBody(request, body, callbackOK, callbackError);
        }
        
        public Task<Response> PostAsync(string url, byte[] bytes)
        {
            var request = InstanceWebRequest(url);
            request.Method = "POST";
            request.ContentType = ContentType;

            AddHeaders(request);

            return CallHttpWithBodyAsync(request, bytes);
        }

        public void Post(string url, byte[] bytes, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            var request = InstanceWebRequest(url);
            request.Method = "POST";
            request.ContentType = ContentType;
            
            
            AddHeaders(request);

            CallHttpWithBody(request, bytes, callbackOK, callbackError);
        }

        #endregion

        #region Private Methods

        private Task<Response> CallHttpAsync(HttpWebRequest request)
        {
            return Task.Run<Response>(async () =>
            {
                try
                {
                    var response = (HttpWebResponse)await request.GetResponseAsync();
                    var streamResponse = response.GetResponseStream();

                    return new Response() { ResponseStream = streamResponse, StatusCode = response.StatusCode };
                }
                catch(WebException e)
                {
                    return new ResponseError(e);
                }
                
            });

        }

        private void CallHttp(HttpWebRequest request, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            try
            {
                request.BeginGetResponse((result) =>
                {
                    try
                    {
                        var responseRequest = (HttpWebRequest)result.AsyncState;
                        var response = (HttpWebResponse)responseRequest.EndGetResponse(result);
                        
                        var streamResponse = response.GetResponseStream();
                        callbackOK.Invoke(new Response() { ResponseStream = streamResponse, StatusCode = response.StatusCode });
                    }
                    catch (WebException e)
                    {
                        callbackError.Invoke(new ResponseError(e));                                                                                                    
                    }
                    catch (Exception e)
                    {
                        callbackError.Invoke(new ResponseError(e));
                    }
                }, request);

            }
            catch (Exception e)
            {
                callbackError.Invoke(new ResponseError(e));
            }
        }

        private Task<Response> CallHttpWithBodyAsync(HttpWebRequest request, byte[] body)
        {
            try
            {
                return Task.Run<Response>(async () =>
                {
                    var requestStream = await request.GetRequestStreamAsync();
                    requestStream.Write(body, 0, body.Length);
                    var response = (HttpWebResponse)await request.GetResponseAsync();
                    var streamResponse = response.GetResponseStream();

                    return new Response() { ResponseStream = streamResponse, StatusCode = response.StatusCode };
                });

            }
            catch
            {
                throw;
            }
        }

        private void CallHttpWithBody(HttpWebRequest request, byte[] body, Action<Response> callbackOK, Action<ResponseError> callbackError)
        {
            try
            {                
                request.BeginGetRequestStream((result) =>
                {
                    var requestFromStream = (HttpWebRequest)(result.AsyncState as object[])[0];
                    var requestBytes = (byte[])(result.AsyncState as object[])[1];

                    Stream endStream = null;
                    
                    try
                    {
                        endStream = requestFromStream.EndGetRequestStream(result);
                        endStream.Write(requestBytes, 0, requestBytes.Length);
                    }
                    catch(WebException e)
                    {
                        callbackError.Invoke(new ResponseError(e));
                    }
                    finally
                    {
                        if(endStream != null)
                        {                            
                            endStream.Flush();
                            endStream.Dispose();
                            endStream = null;                            
                        }                        
                    }                
                                        
                    request.BeginGetResponse((resultResponse) =>
                    {
                        try
                        {                            
                            var requestFromResponse = (HttpWebRequest)resultResponse.AsyncState;
                            var response = (HttpWebResponse)requestFromResponse.EndGetResponse(resultResponse);                                                        

                            var responseStream = response.GetResponseStream();
                            
                                                                                    
                            callbackOK.Invoke(new Response() { ResponseStream = responseStream, StatusCode = response.StatusCode });
                        }
                        catch (WebException e)
                        {
                            callbackError.Invoke(new ResponseError(e));
                        }
                    }, requestFromStream);

                }, new object[] { request, body });
            }
            catch (Exception e)
            {
                callbackError.Invoke(new ResponseError(e));
            }
        }

        private void AddHeaders(HttpWebRequest request)
        {
            foreach (KeyValuePair<string, string> header in Headers)
            {
                request.Headers[header.Key] = header.Value;
            }
        }

        private HttpWebRequest InstanceWebRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                var prop = httpWebRequest.GetType().GetRuntimeProperty("UserAgent");

                prop.SetValue(httpWebRequest, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            }
            catch(Exception e)
            {
                string a = string.Empty;
            }
            

            return httpWebRequest;
        }
        
        #endregion

    }    
}

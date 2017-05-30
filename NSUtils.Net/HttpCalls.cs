﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NSUtils.Net
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

        public void Get(string url, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("GET Request to {0}", new object[] { url }));

            CallHttp(request, callbackOK, callbackError);
        }

        public Task<Response> GetAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("GET Request to {0}", new object[] { url }));

            return CallHttpAsync(request);
        }

        public Task<Response> PostAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("POST Request to {0}", new object[] { url }));

            return CallHttpAsync(request);
        }
        
        public void Post(string url, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
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

        public void Post(string url, string body, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            byte[] data = Encoding.UTF8.GetBytes(body);

            this.Post(url, data, callbackOK, callbackError);
        }

        public Task<Response> DeleteAsync(string url , byte[] body)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("DELETE Request to {0}", new object[] { url }));
            return CallHttpWithBodyAsync(request, body);
        }

        public void Delete(string url, byte[] body, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "DELETE";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("DELETE Request to {0}", new object[] { url }));
            CallHttpWithBody(request, body, callbackOK, callbackError);
        }

        public Task<Response> PutAsync(string url, byte[] body)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("PUT Request to {0}", new object[] { url }));
            return CallHttpWithBodyAsync(request, body);
        }

        public void Put(string url, byte[] body, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = ContentType;

            AddHeaders(request);

            System.Diagnostics.Debug.WriteLine(string.Format("PUT Request to {0}", new object[] { url }));
            CallHttpWithBody(request, body, callbackOK, callbackError);
        }
        
        public Task<Response> PostAsync(string url, byte[] bytes)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;

            AddHeaders(request);

            return CallHttpWithBodyAsync(request, bytes);
        }

        public void Post(string url, byte[] bytes, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = ContentType;

            AddHeaders(request);

            CallHttpWithBody(request, bytes, callbackOK, callbackError);
        }

        #endregion

        #region Private Methods

        private Task<Response> CallHttpAsync(HttpWebRequest request)
        {
            try
            {
                return Task.Run<Response>(async () =>
                {
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

        private void CallHttp(HttpWebRequest request, Action<Response> callbackOK, Action<Exception> callbackError)
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
                        if (e.Response != null)
                        {
                            var errorStream = e.Response.GetResponseStream();
                            string error = new StreamReader(errorStream).ReadToEnd();
                            callbackError.Invoke(e);
                        }
                        else
                        {
                            callbackError.Invoke(new Exception("Network error"));
                        }
                    }
                    catch (Exception e)
                    {
                        callbackError.Invoke(e);
                    }

                }, request);

            }
            catch (Exception e)
            {
                callbackError.Invoke(e);
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

        private void CallHttpWithBody(HttpWebRequest request, byte[] body, Action<Response> callbackOK, Action<Exception> callbackError)
        {
            try
            {
                request.BeginGetRequestStream((result) =>
                {
                    var requestFromStream = (HttpWebRequest)(result.AsyncState as object[])[0];
                    var requestBytes = (byte[])(result.AsyncState as object[])[1];

                    using (var endStream = requestFromStream.EndGetRequestStream(result))
                    {
                        endStream.Write(requestBytes, 0, requestBytes.Length);
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
                            string web = e.Message;
                            Exception ex = e;
                            string message = new StreamReader(e.Response.GetResponseStream()).ReadToEnd();
                            callbackError.Invoke(e);
                        }



                    }, requestFromStream);

                }, new object[] { request, body });
            }
            catch (Exception e)
            {
                callbackError.Invoke(e);
            }
        }

        private void AddHeaders(HttpWebRequest request)
        {
            foreach (KeyValuePair<string, string> header in Headers)
            {
                if (header.Key == "User-Agent")
                {
                    PropertyInfo propertyInfo = request.GetType().GetRuntimeProperty("UserAgent");
                    propertyInfo.SetValue(request, header.Value, null);
                }
                else
                {
                    request.Headers[header.Key] = header.Value;
                }
            }
        }

        #endregion

    }
}

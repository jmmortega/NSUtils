using NSUtils.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NSUtils.Service
{
    public class Logger : ILogger
    {
        private List<string> _report = new List<string>();

        public Action<Exception> OnErrorWithException { get; set; }

        public Action<Exception,string> OnErrorWithExceptionAndMessage { get; set; }

        public Action<string> OnErrorWithMessage { get; set; }

        public Action<string> OnInfo { get; set; }

        public void Error(Exception e)
        {
            Debug.WriteLine(e);            
            if(OnErrorWithException != null)
            {
                OnErrorWithException.Invoke(e);
            }
            _report.Add(e.Message);
        }

        public void Error(string message)
        {
            Debug.WriteLine(message);            
            if(OnErrorWithMessage != null)
            {
                OnErrorWithMessage.Invoke(message);
            }
            _report.Add(message);
        }

        public void Error(string message, Exception e)
        {
            Debug.WriteLine(message, e);
            if(OnErrorWithExceptionAndMessage != null)
            {
                OnErrorWithExceptionAndMessage.Invoke(e, message);
            }
            _report.Add(message);
        }

        public void Info(string message)
        {
            Debug.WriteLine(message);
            if(OnInfo != null)
            {
                OnInfo.Invoke(message);
            }
            _report.Add(message);
        }

        public List<string> ReadReport()
        {
            return _report;
        }        
    }
}

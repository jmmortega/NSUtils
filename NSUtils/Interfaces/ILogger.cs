using System;
using System.Collections.Generic;

namespace NSUtils.Interfaces
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Error(Exception e);
        void Error(string message, Exception e);

        List<string> ReadReport();
        
        Action<Exception> OnErrorWithException { get; set; }
        Action<string> OnErrorWithMessage { get; set; }
        Action<string> OnInfo { get; set; }
    }
}

using System;

namespace DotNetTestNSpec.Logging
{
    // Taken from nunit3-vs-adapter
    public interface ITestLogger
    {
        void Error(string message);
        void Error(string message, Exception ex);
        void Warning(string message);
        void Warning(string message, Exception ex);
        void Info(string message);
    }
}
using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace DotNetTestNSpec.Logging
{
    public sealed class TestLogger : ITestLogger
    {
        private readonly IMessageLogger _messageLogger;
        public TestLogger(IMessageLogger messageLogger) => _messageLogger = messageLogger;
        public void Error(string message)
        {
            _messageLogger.SendMessage(TestMessageLevel.Error, message);
        }

        public void Error(string message, Exception ex)
        {
            _messageLogger.SendMessage(TestMessageLevel.Error, $"{message}. Exception: {ex}");
        }

        public void Info(string message)
        {
            _messageLogger.SendMessage(TestMessageLevel.Informational, message);
        }

        public void Warning(string message)
        {
            _messageLogger.SendMessage(TestMessageLevel.Warning, message);
        }

        public void Warning(string message, Exception ex)
        {
            _messageLogger.SendMessage(TestMessageLevel.Warning, $"{message}. Exception: {ex}");
        }
    }
}
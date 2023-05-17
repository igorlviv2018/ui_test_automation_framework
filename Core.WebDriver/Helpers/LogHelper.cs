using NLog;
using System.Collections.Generic;

namespace Taf.UI.Core.Helpers
{
    public class LogHelper
    {
        private static void WriteToLog(ILogger log, LogLevel level, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                LogEventInfo logEvent = new LogEventInfo(level, log.Name, message);

                log.Log(typeof(LogHelper), logEvent);
            }
        }

        public static void Log(ILogger log, string message, string errMessage="")
        {
            if (string.IsNullOrEmpty(errMessage))
            {
                LogInfo(log, message);
            }
            else
            {
                LogError(log, errMessage);
            }
        }

        public static void LogInfo(ILogger log, string message)
        {
            WriteToLog(log, LogLevel.Info, message);
        }

        public static void LogError(ILogger log, string errMessage)
        {
            WriteToLog(log, LogLevel.Error, errMessage);
        }

        public static void LogResult(ILogger log, string message, string errMessage)
        {
            if (!string.IsNullOrEmpty(errMessage))
            {
                WriteToLog(log, LogLevel.Error, errMessage);
            }
            else
            {
                WriteToLog(log, LogLevel.Info, message);
            }
        }

        public static void LogErrorsIfAny(ILogger log, List<string> errMessages)
        {
            LogError(log, ErrorHelper.ConvertErrorsToString(errMessages));
        }

        public static void LogTestStart(ILogger log, string testDesc)
        {
            LogInfo(log, $"**** {testDesc} test started ****");
        }

        public static void LogTestEnd(ILogger log, List<string> errors, string testDesc)
        {
            string allErrors = string.Join("; ", errors.ToArray());

            bool testPassed = string.IsNullOrEmpty(allErrors);

            if (!testPassed)
            {
                log.Error(allErrors);
            }
            
            LogInfo(log, $"**** {testDesc} test ended (test passed: {testPassed}) ****");
            LogInfo(log, $"----------------------------------------------------------");
        }

        public static void LogTestEnd(ILogger log, string err, string testDesc)
        {
            LogTestEnd(log, new List<string>() { err }, testDesc);
        }
    }
}

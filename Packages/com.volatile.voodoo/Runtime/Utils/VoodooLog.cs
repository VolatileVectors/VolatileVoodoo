using System;
using System.Diagnostics;
using UnityEngine;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace VolatileVoodoo.Utils
{
    public static class VoodooLog
    {
        public static Action<string> LogMessageReceived;

        [Conditional("ENABLE_LOGGING")]
        public static void Info(object message)
        {
            Debug.Log(message);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void InfoFormat(string format, params object[] args)
        {
            Debug.LogFormat(format, args);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogInfo(this Object context, object message)
        {
            Debug.Log(message, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogInfoFormat(this Object context, string format, params object[] args)
        {
            Debug.LogFormat(format, args, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void Warning(object message)
        {
            Debug.LogWarning(message);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void WarningFormat(string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarning(this Object context, object message)
        {
            Debug.LogWarning(message, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarningFormat(this Object context, string format, params object[] args)
        {
            Debug.LogWarningFormat(format, args, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void Error(object message)
        {
            Debug.LogError(message);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void ErrorFormat(string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogError(this Object context, object message)
        {
            Debug.LogError(message, context);
        }

        [Conditional("ENABLE_LOGGING")]
        public static void LogErrorFormat(this Object context, string format, params object[] args)
        {
            Debug.LogErrorFormat(format, args, context);
        }

#if !ENABLE_LOGGING
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void DisableLogging()
        {
            Debug.unityLogger.logEnabled = false; // Turn off logging (for plugin/assets)
            // Debug.unityLogger.filterLogType = LogType.Error;

        }
#else
        private const string ColorLog = "#DAF7A6";
        private const string ColorWarning = "#FFC300";
        private const string ColorError = "#FF5733";
        private const string ColorAssert = "#C70039";
        private const string ColorException = "#900C3F";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitLogMessageFormater()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private static void OnLogMessageReceived(string logMessage, string stacktrace, LogType logType)
        {
            LogMessageReceived?.Invoke(logType switch
                {
                    LogType.Log => $"<color={ColorLog}>",
                    LogType.Warning => $"<color={ColorWarning}>",
                    LogType.Error => $"<color={ColorError}>",
                    LogType.Assert => $"<color={ColorAssert}>",
                    LogType.Exception => $"<color={ColorException}>",
                    _ => ""
                } + $"[{DateTime.Now.ToLongTimeString()}] {logMessage}</color>"
            );
        }
#endif
    }
}
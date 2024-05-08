using System;
using System.Collections.Generic;
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
        private const int LogMessageBufferSize = 250;
        public static Queue<string> LogMessageBuffer { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitLogMessageBuffer()
        {
            LogMessageBuffer = new Queue<string>(LogMessageBufferSize);
            Application.logMessageReceived += OnLogMessageReceived;
        }

        private static void OnLogMessageReceived(string logMessage, string stacktrace, LogType logType)
        {
            if (LogMessageBuffer.Count >= 50)
            {
                LogMessageBuffer.Dequeue();
            }

            var message = logType switch
            {
                LogType.Error => "ERROR - ",
                LogType.Assert => "ASSERT - ",
                LogType.Warning => "WARNING - ",
                LogType.Log => "LOG - ",
                LogType.Exception => "EXCEPTION - ",
                _ => ""
            } + logMessage;
            LogMessageBuffer.Enqueue(message);
            LogMessageReceived?.Invoke(message);
        }

        public static string GetLog => string.Join("\n", LogMessageBuffer);
#endif
    }
}
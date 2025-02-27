using System;
using System.Diagnostics;
using UnityEngine;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Capybutler
{
    public static class LogButler
    {
        private static Action<string> logMessageReceived;

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void SubscribeLogMessagesChannel(Action<string> subscriber) => logMessageReceived += subscriber;

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void UnsubscribeLogMessagesChannel(Action<string> subscriber) => logMessageReceived -= subscriber;

#if ENABLE_LOGGING || ENABLE_REPORTING
        private const string ColorLog = "#DAF7A6";
        private const string ColorWarning = "#FFC300";
        private const string ColorAssert = "#C70039";
        private const string ColorError = "#FF5733";
        private const string ColorException = "#900C3F";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitLogMessageFormater() => Application.logMessageReceived += OnLogMessageReceived;

#if !ENABLE_LOGGING
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void COnfigure() => Debug.unityLogger.filterLogType = LogType.Error;
#endif

        private static void OnLogMessageReceived(string logMessage, string stacktrace, LogType logType) =>
            logMessageReceived?.Invoke(logType switch
                {
                    LogType.Log => $"<color={ColorLog}>",
                    LogType.Warning => $"<color={ColorWarning}>",
                    LogType.Assert => $"<color={ColorAssert}>",
                    LogType.Error => $"<color={ColorError}>",
                    LogType.Exception => $"<color={ColorException}>",
                    _ => "<color=#000000>"
                } + $"[{DateTime.Now.ToLongTimeString()}] {logMessage}</color>"
            );
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void DisableLogging()=> Debug.unityLogger.logEnabled = false; // Turn off logging (including plugin/assets)
#endif

        [Conditional("ENABLE_LOGGING")]
        public static void Info(object message) => Debug.Log(message);

        [Conditional("ENABLE_LOGGING")]
        public static void InfoFormat(string format, params object[] args) => Debug.LogFormat(format, args);

        [Conditional("ENABLE_LOGGING")]
        public static void LogInfo(this Object context, object message) => Debug.Log(message, context);

        [Conditional("ENABLE_LOGGING")]
        public static void LogInfoFormat(this Object context, string format, params object[] args) => Debug.LogFormat(format, args, context);

        [Conditional("ENABLE_LOGGING")]
        public static void Warning(object message) => Debug.LogWarning(message);

        [Conditional("ENABLE_LOGGING")]
        public static void WarningFormat(string format, params object[] args) => Debug.LogWarningFormat(format, args);

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarning(this Object context, object message) => Debug.LogWarning(message, context);

        [Conditional("ENABLE_LOGGING")]
        public static void LogWarningFormat(this Object context, string format, params object[] args) => Debug.LogWarningFormat(format, args, context);

        [Conditional("ENABLE_LOGGING")]
        public static void Assert(object message) => Debug.LogAssertion(message);

        [Conditional("ENABLE_LOGGING")]
        public static void AssertFormat(string format, params object[] args) => Debug.LogAssertionFormat(format, args);

        [Conditional("ENABLE_LOGGING")]
        public static void LogAssert(this Object context, object message) => Debug.LogAssertion(message, context);

        [Conditional("ENABLE_LOGGING")]
        public static void LogAssertFormat(this Object context, string format, params object[] args) => Debug.LogAssertionFormat(format, args, context);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void Error(object message) => Debug.LogError(message);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void ErrorFormat(string format, params object[] args) => Debug.LogErrorFormat(format, args);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void LogError(this Object context, object message) => Debug.LogError(message, context);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void LogErrorFormat(this Object context, string format, params object[] args) => Debug.LogErrorFormat(format, args, context);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void Exception(Exception exception) => Debug.LogException(exception);

        [Conditional("ENABLE_LOGGING"), Conditional("ENABLE_REPORTING")]
        public static void LogException(this Object context, Exception exception) => Debug.LogException(exception, context);
    }
}
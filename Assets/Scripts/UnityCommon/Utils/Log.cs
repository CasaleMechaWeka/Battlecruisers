using System;
using System.Linq;
using UnityEngine;

namespace UnityCommon.Utils
{
    public static class Log
    {
        public static void Normal(
            string message = "",
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string logMessage = CreateMessage(message, memberName, sourceFilePath, sourceLineNumber);
            Debug.unityLogger.Log(logMessage);
        }

        public static void Warn(
            string message = "",
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string logMessage = CreateMessage(message, memberName, sourceFilePath, sourceLineNumber);
            Debug.unityLogger.LogWarning(string.Empty, logMessage);
        }

        public static void Error(
            string message = "",
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string logMessage = CreateMessage(message, memberName, sourceFilePath, sourceLineNumber);
            Debug.unityLogger.LogError(string.Empty, logMessage);
        }

        private static string CreateMessage(string message, string memberName, string sourceFilePath, int sourceLineNumber)
        {
            string timestamp = DateTime.Now.ToString("hh:mm:ss.fff");
            string fileName = sourceFilePath.Split('\\').Last();
            return $"{timestamp}-{fileName}:{memberName}[{sourceLineNumber}]: {message}";
        }
    }
}
using UnityEngine;
using System;
using System.IO;

namespace BattleCruisers.Utils
{
    /// <summary>
    /// Captures ALL Unity Console output (Debug.Log, exceptions, errors, warnings)
    /// and writes them to RuntimeConsole.log in the project root.
    /// This ensures Claude can see runtime errors that don't appear in Editor.log.
    /// Only active when ENABLE_LOGS scripting define symbol is set.
    /// </summary>
    public static class ConsoleToFileCapture
    {
        #if ENABLE_LOGS
        private static string logFilePath;
        private static StreamWriter logWriter;
        private static object lockObject = new object();
        #endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            #if !ENABLE_LOGS
            return; // Disable console capture when ENABLE_LOGS is not defined
            #endif

            #if ENABLE_LOGS
            // Create log file path - use persistentDataPath for cross-platform compatibility
            // On Android: /storage/emulated/0/Android/data/com.package/files/
            // On iOS: Application sandbox Documents folder
            // In Editor: Project root
            #if UNITY_EDITOR
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            logFilePath = Path.Combine(projectRoot, "RuntimeConsole.log");
            #else
            logFilePath = Path.Combine(Application.persistentDataPath, "RuntimeConsole.log");
            #endif

            // Initialize file with header
            try
            {
                logWriter = new StreamWriter(logFilePath, false); // false = overwrite
                logWriter.AutoFlush = true;
                string header = $"=== RUNTIME CONSOLE LOG - Started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n";
                header += $"Unity Version: {Application.unityVersion}\n";
                header += $"Product: {Application.productName}\n";
                header += $"Platform: {Application.platform}\n";
                header += "==========================================\n\n";
                logWriter.Write(header);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create RuntimeConsole.log: {e}");
                return;
            }

            // Hook into Unity's log system
            Application.logMessageReceived += OnLogMessageReceived;

            Debug.Log("ConsoleToFileCapture: Log capture started. Writing to " + logFilePath);
            #endif
        }

        #if ENABLE_LOGS
        private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (logWriter == null) return;

            lock (lockObject)
            {
                try
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                    string typeStr = type.ToString().ToUpper();

                    // Write log entry
                    logWriter.WriteLine($"{timestamp} [{typeStr}] {condition}");

                    // Add stack trace for errors and exceptions
                    if (!string.IsNullOrEmpty(stackTrace) &&
                        (type == LogType.Error || type == LogType.Exception || type == LogType.Assert))
                    {
                        logWriter.WriteLine(stackTrace);
                    }

                    logWriter.WriteLine(); // Blank line for readability
                }
                catch (Exception e)
                {
                    // Can't log this error to Unity console as it would cause recursion
                    // Just print to stderr
                    Console.Error.WriteLine($"ConsoleToFileCapture error: {e}");
                }
            }
        }
        #endif

        // Cleanup on application quit
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RegisterCleanup()
        {
            #if ENABLE_LOGS
            Application.quitting += OnApplicationQuit;
            #endif
        }

        #if ENABLE_LOGS
        private static void OnApplicationQuit()
        {
            if (logWriter != null)
            {
                lock (lockObject)
                {
                    try
                    {
                        logWriter.WriteLine($"\n=== Log Ended {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
                        logWriter.Close();
                        logWriter = null;
                    }
                    catch { }
                }
            }

            Application.logMessageReceived -= OnLogMessageReceived;
        }
        #endif
    }
}

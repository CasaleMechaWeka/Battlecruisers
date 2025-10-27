using UnityEngine;
using System;
using System.IO;

namespace BattleCruisers.Utils
{
    /// <summary>
    /// Captures ALL Unity Console output (Debug.Log, exceptions, errors, warnings)
    /// and writes them to RuntimeConsole.log in the project root.
    /// This ensures Claude can see runtime errors that don't appear in Editor.log.
    /// </summary>
    public static class ConsoleToFileCapture
    {
        private static string logFilePath;
        private static StreamWriter logWriter;
        private static object lockObject = new object();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Create log file path (project root, next to Assets folder)
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            logFilePath = Path.Combine(projectRoot, "RuntimeConsole.log");

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
        }

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

        // Cleanup on application quit
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void RegisterCleanup()
        {
            Application.quitting += OnApplicationQuit;
        }

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
    }
}

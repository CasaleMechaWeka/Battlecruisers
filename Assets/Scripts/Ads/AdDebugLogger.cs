using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace BattleCruisers.Ads
{
    /// <summary>
    /// Centralized debug logger for ad system - writes to persistent file
    /// File location: Application.persistentDataPath/AdDebugLog.txt
    /// </summary>
    public class AdDebugLogger : MonoBehaviour
    {
        public static AdDebugLogger Instance { get; private set; }

        private string logFilePath;
        private StringBuilder logBuffer = new StringBuilder();
        private bool isEnabled = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up log file path
            logFilePath = Path.Combine(Application.persistentDataPath, "AdDebugLog.txt");
            
            // Clear old log and start fresh
            try
            {
                File.WriteAllText(logFilePath, "");
                Log("=== AD DEBUG LOG STARTED ===");
                Log($"Unity Version: {Application.unityVersion}");
                Log($"Platform: {Application.platform}");
                Log($"Device Model: {SystemInfo.deviceModel}");
                Log($"OS: {SystemInfo.operatingSystem}");
                Log($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Log($"Log File: {logFilePath}");
                Log("================================\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdDebugLogger] Failed to initialize log file: {e.Message}");
                isEnabled = false;
            }
        }

        /// <summary>
        /// Log a message to both Unity console and file
        /// Note: Messages tagged with [AdLog] will appear in Android logcat
        /// </summary>
        public void Log(string message)
        {
            if (!isEnabled) return;

            string timestamped = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
            
            // Write to Unity console (this appears in logcat on Android)
            Debug.Log($"[AdLog] {timestamped}");

            // Write to file
            try
            {
                logBuffer.AppendLine(timestamped);
                
                // Flush to file every message for reliability
                File.AppendAllText(logFilePath, timestamped + "\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdDebugLogger] Failed to write to file: {e.Message}");
            }
        }

        /// <summary>
        /// Log a warning to both Unity console and file
        /// </summary>
        public void LogWarning(string message)
        {
            if (!isEnabled) return;

            string timestamped = $"[{DateTime.Now:HH:mm:ss.fff}] WARNING: {message}";
            
            Debug.LogWarning($"[AdLog] {timestamped}");

            try
            {
                logBuffer.AppendLine(timestamped);
                File.AppendAllText(logFilePath, timestamped + "\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdDebugLogger] Failed to write to file: {e.Message}");
            }
        }

        /// <summary>
        /// Log an error to both Unity console and file
        /// </summary>
        public void LogError(string message)
        {
            if (!isEnabled) return;

            string timestamped = $"[{DateTime.Now:HH:mm:ss.fff}] ERROR: {message}";
            
            Debug.LogError($"[AdLog] {timestamped}");

            try
            {
                logBuffer.AppendLine(timestamped);
                File.AppendAllText(logFilePath, timestamped + "\n");
            }
            catch (Exception e)
            {
                Debug.LogError($"[AdDebugLogger] Failed to write to file: {e.Message}");
            }
        }

        /// <summary>
        /// Log a section header
        /// </summary>
        public void LogSection(string title)
        {
            Log($"\n=== {title} ===");
        }

        /// <summary>
        /// Get the log file path (for reference)
        /// </summary>
        public string GetLogFilePath()
        {
            return logFilePath;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Log("=== AD DEBUG LOG ENDED ===\n");
                Instance = null;
            }
        }
    }
}


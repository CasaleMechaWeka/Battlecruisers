using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    /// <summary>
    /// Collects detailed logs for AppLovin support debugging
    /// Specifically for close button issues and ad display problems
    /// </summary>
    public class AppLovinLogCollector : MonoBehaviour
    {
        private static AppLovinLogCollector _instance;
        public static AppLovinLogCollector Instance => _instance;

        private readonly List<string> _logs = new List<string>();
        private readonly int _maxLogs = 500;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            Application.logMessageReceived += HandleLog;
            LogSystemInfo();
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Only capture AppLovin, ad-related, and error logs
            if (logString.Contains("AppLovin") || 
                logString.Contains("INTERSTITIAL") || 
                logString.Contains("REWARDED") ||
                logString.Contains("rt.applovin.com") ||
                logString.Contains("MAX SDK") ||
                type == LogType.Error || 
                type == LogType.Exception)
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
                string entry = $"[{timestamp}] [{type}] {logString}";
                
                if (type == LogType.Exception && !string.IsNullOrEmpty(stackTrace))
                {
                    entry += $"\n{stackTrace}";
                }
                
                _logs.Add(entry);
                
                // Keep only recent logs
                if (_logs.Count > _maxLogs)
                {
                    _logs.RemoveAt(0);
                }
            }
        }

        private void LogSystemInfo()
        {
            AddCustomLog("=== SYSTEM INFORMATION ===");
            AddCustomLog($"Unity Version: {Application.unityVersion}");
            AddCustomLog($"Device Model: {SystemInfo.deviceModel}");
            AddCustomLog($"OS: {SystemInfo.operatingSystem}");
            AddCustomLog($"RAM: {SystemInfo.systemMemorySize} MB");
            AddCustomLog($"GPU: {SystemInfo.graphicsDeviceName}");
            AddCustomLog($"Screen: {Screen.width}x{Screen.height} @ {Screen.currentResolution.refreshRate}Hz");
            AddCustomLog($"Network: {Application.internetReachability}");
            AddCustomLog($"Build Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
#if UNITY_ANDROID
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                AddCustomLog($"Android SDK: {version.GetStatic<int>("SDK_INT")}");
                AddCustomLog($"Android Release: {version.GetStatic<string>("RELEASE")}");
            }
#endif
            
            AddCustomLog("=========================\n");
        }

        public void AddCustomLog(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _logs.Add($"[{timestamp}] [Info] {message}");
        }

        public void LogAdAttempt(string adType, string action)
        {
            AddCustomLog($">>> AD EVENT: {adType} - {action}");
        }

        public string GetFullLog()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=================================================");
            sb.AppendLine("BATTLECRUISERS - APPLOVIN DEBUG LOG");
            sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("Issue: Ad close button not appearing");
            sb.AppendLine("=================================================\n");
            
            foreach (var log in _logs)
            {
                sb.AppendLine(log);
            }
            
            sb.AppendLine("\n=== END OF LOG ===");
            return sb.ToString();
        }

        public void SaveLogToFile()
        {
            try
            {
                string filename = $"AppLovin_Debug_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
                System.IO.File.WriteAllText(path, GetFullLog());
                
                Debug.Log($"[LogCollector] Log saved to: {path}");
                AddCustomLog($"Log file saved: {filename}");
                
#if UNITY_ANDROID && !UNITY_EDITOR
                // Copy to external storage if available
                try
                {
                    string externalPath = "/sdcard/Download/" + filename;
                    System.IO.File.WriteAllText(externalPath, GetFullLog());
                    Debug.Log($"[LogCollector] Also saved to: {externalPath}");
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[LogCollector] Could not save to external storage: {ex.Message}");
                }
#endif
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LogCollector] Failed to save log: {ex.Message}");
            }
        }

        public void ClearLogs()
        {
            _logs.Clear();
            LogSystemInfo();
            AddCustomLog(">>> Log cleared by user");
        }

        // Call this from AdminPanel when testing ads
        public void MarkAdTestStart(string adType)
        {
            AddCustomLog($"\n========================================");
            AddCustomLog($"TEST START: {adType} Ad");
            AddCustomLog($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            AddCustomLog($"========================================");
        }

        public void MarkAdTestEnd(string adType, bool success)
        {
            AddCustomLog($"========================================");
            AddCustomLog($"TEST END: {adType} Ad - {(success ? "SUCCESS" : "FAILED")}");
            AddCustomLog($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            AddCustomLog($"========================================\n");
        }
    }
}


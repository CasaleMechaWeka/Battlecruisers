using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System;
using System.Linq;

public class ErrorLogScraper : EditorWindow
{
    private Vector2 _scrollPosition;
    private List<string> _scrapedErrors = new List<string>();
    private bool _includeUnityConsole = true;
    private bool _includeLogcat = true;
    private bool _showDebugInfo = true;
    private string _debugMessage = "";
    private bool _isScraping = false;

    [MenuItem("Tools/Error Log Scraper")]
    public static void ShowWindow()
    {
        GetWindow<ErrorLogScraper>("Error Scraper");
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        _includeUnityConsole = EditorGUILayout.Toggle("Include Unity Console", _includeUnityConsole);
        _includeLogcat = EditorGUILayout.Toggle("Include ADB Logcat", _includeLogcat);
        _showDebugInfo = EditorGUILayout.Toggle("Show Debug Info", _showDebugInfo);

        EditorGUILayout.Space();

        if (GUILayout.Button("Scrape Errors"))
        {
            ScrapeErrors();
        }

        EditorGUILayout.Space();
        
        if (_showDebugInfo && !string.IsNullOrEmpty(_debugMessage))
        {
             EditorGUILayout.HelpBox(_debugMessage, MessageType.Info);
        }

        GUILayout.Label($"Found Errors: {_scrapedErrors.Count}", EditorStyles.boldLabel);

        if (_scrapedErrors.Count > 0)
        {
            if (GUILayout.Button("Copy to Clipboard"))
            {
                CopyToClipboard();
            }

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < _scrapedErrors.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{i + 1}.", GUILayout.Width(30));
                EditorGUILayout.TextArea(_scrapedErrors[i], EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void ScrapeErrors()
    {
        _scrapedErrors.Clear();
        _debugMessage = "";
        _isScraping = true;
        
        var sb = new StringBuilder();

        try
        {
            if (_includeUnityConsole)
            {
                ScrapeUnityConsole(sb);
            }

            if (_includeLogcat)
            {
                ScrapeLogcat(sb);
            }
        }
        catch (Exception ex)
        {
            sb.AppendLine($"Critical Error: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            _isScraping = false;
            _debugMessage = sb.ToString();
        }
    }

    private void ScrapeUnityConsole(StringBuilder debugLog)
    {
        try
        {
            // Use Assembly.GetAssembly(typeof(UnityEditor.Editor)) to be safer about finding the assembly
            var unityEditorAssembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
            
            if (logEntriesType == null)
            {
                debugLog.AppendLine("Could not find UnityEditor.LogEntries type");
                return;
            }

            var getCountMethod = logEntriesType.GetMethod("GetCount");
            var startGettingEntriesMethod = logEntriesType.GetMethod("StartGettingEntries");
            var endGettingEntriesMethod = logEntriesType.GetMethod("EndGettingEntries");
            var getEntryInternalMethod = logEntriesType.GetMethod("GetEntryInternal");

            var logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");
            if (logEntryType == null)
            {
                debugLog.AppendLine("Could not find UnityEditor.LogEntry type");
                return;
            }

            var logEntry = Activator.CreateInstance(logEntryType);
            
            var messageField = logEntryType.GetField("message");
            var modeField = logEntryType.GetField("mode");

            if (startGettingEntriesMethod != null)
                startGettingEntriesMethod.Invoke(null, null);

            int count = (int)getCountMethod.Invoke(null, null);
            debugLog.AppendLine($"Unity Console Total Log Count: {count}");

            int errorCount = 0;
            for (int i = 0; i < count; i++)
            {
                getEntryInternalMethod.Invoke(null, new object[] { i, logEntry });
                
                int mode = (int)modeField.GetValue(logEntry);
                string message = (string)messageField.GetValue(logEntry);

                // ConsoleWindow.Mode flags:
                // Error = 1
                // Assert = 2
                // Fatal = 16
                // AssetImportError = 64
                // ScriptingError = 256
                // Log = 4
                // Warning = ? (usually 4 is log, warning might be 8 or mixed)
                
                // Let's include everything for debugging if needed, but filter for errors per request.
                // If mode has ANY of the error bits set, we take it.
                bool isError = (mode & (1 | 2 | 16 | 64 | 256)) != 0;
                
                if (isError)
                {
                    AddError(message);
                    errorCount++;
                }
            }
            debugLog.AppendLine($"Unity Console Errors Found: {errorCount}");

            if (endGettingEntriesMethod != null)
                endGettingEntriesMethod.Invoke(null, null);
        }
        catch (Exception e)
        {
            debugLog.AppendLine($"Error scraping Unity console: {e.Message}");
            UnityEngine.Debug.LogError($"Error scraping Unity console: {e.Message}");
        }
    }

    private void ScrapeLogcat(StringBuilder debugLog)
    {
        try
        {
            string adbPath = "adb";
            string sdkRoot = EditorPrefs.GetString("AndroidSdkRoot");
            if (!string.IsNullOrEmpty(sdkRoot))
            {
                string platformTools = Path.Combine(sdkRoot, "platform-tools", "adb");
                if (File.Exists(platformTools) || File.Exists(platformTools + ".exe"))
                {
                    adbPath = platformTools;
                }
            }
            
            debugLog.AppendLine($"Using ADB Path: {adbPath}");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = adbPath,
                Arguments = "logcat -d *:E", // Dump errors only
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();
                
                process.WaitForExit(2000); // Wait up to 2 seconds

                if (process.ExitCode == 0)
                {
                    string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    debugLog.AppendLine($"ADB Logcat lines read: {lines.Length}");
                    
                    int addedCount = 0;
                    foreach (var line in lines)
                    {
                        if (line.Contains(" E ") || line.StartsWith("E/"))
                        {
                            AddError(line);
                            addedCount++;
                        }
                    }
                    debugLog.AppendLine($"ADB Logcat Errors added: {addedCount}");
                }
                else
                {
                     debugLog.AppendLine($"ADB process exit code: {process.ExitCode}");
                     if (!string.IsNullOrEmpty(errorOutput))
                        debugLog.AppendLine($"ADB Error Output: {errorOutput}");
                }
            }
        }
        catch (Exception e)
        {
            debugLog.AppendLine($"Could not scrape Logcat: {e.Message}");
        }
    }

    private void AddError(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        // Truncate to 256 chars
        string truncated = message.Length > 256 ? message.Substring(0, 256) + "..." : message;
        
        // Clean up newlines if any, to keep it as a list item
        truncated = truncated.Replace("\n", " ").Replace("\r", "");
        
        // Avoid duplicates if desired, or keep all
        _scrapedErrors.Add(truncated);
    }

    private void CopyToClipboard()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _scrapedErrors.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {_scrapedErrors[i]}");
        }
        EditorGUIUtility.systemCopyBuffer = sb.ToString();
        UnityEngine.Debug.Log("Errors copied to clipboard!");
    }
}

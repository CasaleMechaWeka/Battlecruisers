using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class ErrorLogScraper : EditorWindow
{
    private Vector2 _scrollPosition;
    private List<string> _scrapedErrors = new List<string>();
    private bool _includeUnityConsole = true;
    private bool _includeLogcat = true;
    private bool _showDebugInfo = true;
    private string _debugMessage = "";
    
    // Log type filters
    private bool _includeErrors = true;
    private bool _includeWarnings = false;
    private bool _includeMessages = false;

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
        GUILayout.Label("Log Type Filters", EditorStyles.boldLabel);
        _includeErrors = EditorGUILayout.Toggle("Include Errors", _includeErrors);
        _includeWarnings = EditorGUILayout.Toggle("Include Warnings", _includeWarnings);
        _includeMessages = EditorGUILayout.Toggle("Include Messages/Info", _includeMessages);

        EditorGUILayout.Space();

        if (GUILayout.Button("Scrape Logs"))
        {
            ScrapeErrors();
        }

        EditorGUILayout.Space();
        
        if (_showDebugInfo && !string.IsNullOrEmpty(_debugMessage))
        {
             EditorGUILayout.HelpBox(_debugMessage, MessageType.Info);
        }

        GUILayout.Label($"Found Logs: {_scrapedErrors.Count}", EditorStyles.boldLabel);

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
            
            // Try GetEntry method as well (might be more reliable)
            var getEntryMethod = logEntriesType.GetMethod("GetEntry");

            var logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");
            if (logEntryType == null)
            {
                debugLog.AppendLine("Could not find UnityEditor.LogEntry type");
                return;
            }

            var logEntry = Activator.CreateInstance(logEntryType);
            
            // Get all available fields to understand the structure
            var messageField = logEntryType.GetField("message");
            var modeField = logEntryType.GetField("mode");
            var conditionField = logEntryType.GetField("condition");
            
            // Try to find a field that gives us the LogType directly
            var logTypeField = logEntryType.GetField("type");
            if (logTypeField == null)
            {
                logTypeField = logEntryType.GetField("logType");
            }

            if (startGettingEntriesMethod != null)
                startGettingEntriesMethod.Invoke(null, null);

            int count = (int)getCountMethod.Invoke(null, null);
            debugLog.AppendLine($"Unity Console Total Log Count: {count}");

            int addedCount = 0;
            int errorCount = 0;
            int warningCount = 0;
            int messageCount = 0;
            
            for (int i = 0; i < count; i++)
            {
                // Try GetEntry first, fallback to GetEntryInternal
                if (getEntryMethod != null)
                {
                    try
                    {
                        getEntryMethod.Invoke(null, new object[] { i, logEntry });
                    }
                    catch
                    {
                        getEntryInternalMethod.Invoke(null, new object[] { i, logEntry });
                    }
                }
                else
                {
                    getEntryInternalMethod.Invoke(null, new object[] { i, logEntry });
                }
                
                int mode = (int)modeField.GetValue(logEntry);
                
                // Prefer condition field (first line) over message field (full text)
                string message = conditionField != null ? (string)conditionField.GetValue(logEntry) : null;
                if (string.IsNullOrEmpty(message))
                {
                    message = (string)messageField.GetValue(logEntry);
                }

                // Try to get LogType directly if available
                LogType? logType = null;
                if (logTypeField != null)
                {
                    try
                    {
                        logType = (LogType)logTypeField.GetValue(logEntry);
                    }
                    catch { }
                }

                bool isError = false;
                bool isWarning = false;
                bool isMessage = false;

                if (logType.HasValue)
                {
                    // Use LogType enum directly if available
                    isError = logType.Value == LogType.Error || 
                              logType.Value == LogType.Exception || 
                              logType.Value == LogType.Assert;
                    isWarning = logType.Value == LogType.Warning;
                    isMessage = logType.Value == LogType.Log;
                }
                else
                {
                    // Fallback to mode bit checking
                    // Error types: 1 (Error), 2 (Assert), 16 (Fatal), 64 (AssetImportError), 256 (ScriptingError)
                    isError = (mode & (1 | 2 | 16 | 64 | 256)) != 0;
                    
                    // Warning detection: Unity's mode values can vary
                    // Based on Unity versions:
                    // - Mode 4 is often used for both Warning and Log
                    // - Mode 8 can be Warning in some versions
                    // - The distinction is subtle and version-dependent
                    // Strategy: If warnings are enabled, be more inclusive
                    if (!isError)
                    {
                        // Check various possible warning patterns
                        // Mode 4 alone is often a warning (but can be log)
                        // Mode 8 is typically warning
                        // Mode with 4 bit set but no error bits might be warning
                        if (mode == 4)
                        {
                            // Mode 4 alone - could be warning or log
                            // If user wants warnings, include it as warning
                            // If user wants messages, also include as message
                            // We'll prioritize warning if both are enabled
                            if (_includeWarnings)
                            {
                                isWarning = true;
                            }
                            else if (_includeMessages)
                            {
                                isMessage = true;
                            }
                        }
                        else if (mode == 8 || (mode & 8) != 0)
                        {
                            // Mode 8 is typically warning
                            isWarning = true;
                        }
                        else if ((mode & 4) != 0)
                        {
                            // Mode has 4 bit but other bits too - likely a log
                            isMessage = true;
                        }
                        else
                        {
                            // Other non-error modes - treat as message
                            isMessage = true;
                        }
                    }
                }
                
                // Debug: log mode values to understand the pattern
                // When warnings are enabled, log more entries to see what mode values warnings actually have
                if (_showDebugInfo)
                {
                    if (i < 20) // Log first 20 for debugging
                    {
                        debugLog.AppendLine($"Log {i}: mode={mode}, logType={logType}, msg={message?.Substring(0, Math.Min(50, message?.Length ?? 0))}...");
                    }
                    // Also log any entry that has mode 4 or 8 (potential warnings)
                    if ((mode == 4 || mode == 8) && i < 100)
                    {
                        debugLog.AppendLine($"  -> Mode {mode} entry: {message?.Substring(0, Math.Min(80, message?.Length ?? 0))}...");
                    }
                }
                
                bool shouldInclude = false;
                if (_includeErrors && isError) 
                {
                    shouldInclude = true;
                    errorCount++;
                }
                else if (_includeWarnings && isWarning) 
                {
                    shouldInclude = true;
                    warningCount++;
                }
                else if (_includeMessages && isMessage) 
                {
                    shouldInclude = true;
                    messageCount++;
                }
                
                if (shouldInclude)
                {
                    AddLogEntry(message);
                    addedCount++;
                }
            }
            debugLog.AppendLine($"Unity Console Logs Added: {addedCount} (Errors: {errorCount}, Warnings: {warningCount}, Messages: {messageCount})");

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

            // Build logcat filter based on selected log types
            // Get all logs and filter in C# for more control
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = adbPath,
                Arguments = "logcat -d -v time", // Dump all logs with timestamps
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string errorOutput = process.StandardError.ReadToEnd();
                
                process.WaitForExit(5000); // Give more time for large logs

                if (process.ExitCode == 0)
                {
                    string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    debugLog.AppendLine($"ADB Logcat lines read: {lines.Length}");
                    
                    int addedCount = 0;
                    foreach (var line in lines)
                    {
                        bool shouldInclude = false;
                        
                        // Logcat format: "12-11 15:39:00.000 1234 5678 E Tag: Message"
                        // Or: "E/Tag   (1234): Message"
                        if (_includeErrors && (line.Contains(" E ") || line.StartsWith("E/") || line.Contains(" E/")))
                        {
                            shouldInclude = true;
                        }
                        else if (_includeWarnings && (line.Contains(" W ") || line.StartsWith("W/") || line.Contains(" W/")))
                        {
                            shouldInclude = true;
                        }
                        else if (_includeMessages && (line.Contains(" I ") || line.StartsWith("I/") || line.Contains(" I/")))
                        {
                            shouldInclude = true;
                        }
                        
                        if (shouldInclude)
                        {
                            AddLogEntry(line);
                            addedCount++;
                        }
                    }
                    debugLog.AppendLine($"ADB Logcat Logs added: {addedCount}");
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

    private void AddLogEntry(string message)
    {
        if (string.IsNullOrEmpty(message)) return;

        // Extract only the first line (before any newline)
        string firstLine = message;
        int newlineIndex = message.IndexOfAny(new[] { '\n', '\r' });
        if (newlineIndex >= 0)
        {
            firstLine = message.Substring(0, newlineIndex);
        }
        
        // Extract file and line information from the first line
        string filePath = null;
        string lineNumber = null;
        
        // Pattern 1: (at Path/To/File.cs:123) or (at Path/To/File.cs:123:0)
        Match match = Regex.Match(firstLine, @"\(at\s+([^:)]+):(\d+)");
        if (match.Success)
        {
            filePath = match.Groups[1].Value;
            lineNumber = match.Groups[2].Value;
        }
        else
        {
            // Pattern 2: File.cs:123 (without "at")
            match = Regex.Match(firstLine, @"([A-Za-z0-9_/\\]+\.(cs|js|ts|cpp|h)):(\d+)");
            if (match.Success)
            {
                filePath = match.Groups[1].Value;
                lineNumber = match.Groups[3].Value;
            }
        }
        
        // Build the output string with file and line info
        string truncated = firstLine.Length > 256 ? firstLine.Substring(0, 256) + "..." : firstLine;
        truncated = truncated.Trim();
        
        // Prepend file and line info if found
        if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(lineNumber))
        {
            // Extract just the filename if path is long
            string fileName = Path.GetFileName(filePath);
            string displayPath = filePath.Length > 50 ? fileName : filePath;
            truncated = $"[{displayPath}:{lineNumber}] {truncated}";
        }
        
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

#if UNITY_ANDROID
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor.Android;

/// <summary>
/// Adds google-services plugin for Firebase. Required when using Firebase via direct JNI calls.
/// </summary>
public class FixFirebaseGoogleServices : IPostGenerateGradleAndroidProject
{
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        // Copy google-services.json to launcher module
        var googleServicesSource = Path.Combine(Application.dataPath, "google-services.json");
        if (File.Exists(googleServicesSource))
        {
            var launcherSrcDir = Path.Combine(path, "../launcher/src");
            if (!Directory.Exists(launcherSrcDir))
            {
                Directory.CreateDirectory(launcherSrcDir);
            }
            var googleServicesDest = Path.Combine(launcherSrcDir, "google-services.json");
            File.Copy(googleServicesSource, googleServicesDest, true);
            UnityEngine.Debug.Log($"[FixFirebaseGoogleServices] Copied google-services.json to {googleServicesDest}");
        }
        else
        {
            UnityEngine.Debug.LogWarning($"[FixFirebaseGoogleServices] google-services.json not found at {googleServicesSource}");
        }

        // Add classpath to root build.gradle
        var rootGradle = Path.Combine(path, "../build.gradle");
        if (File.Exists(rootGradle))
        {
            var rootLines = File.ReadAllLines(rootGradle).ToList();
            if (!rootLines.Any(l => l.Contains("google-services")))
            {
                // Find buildscript block or add it
                var hasBuildscript = rootLines.Any(l => l.Contains("buildscript"));
                if (!hasBuildscript)
                {
                    rootLines.Insert(0, "buildscript {");
                    rootLines.Insert(1, "    repositories { google(); mavenCentral() }");
                    rootLines.Insert(2, "    dependencies { classpath 'com.google.gms:google-services:4.4.0' }");
                    rootLines.Insert(3, "}");
                    rootLines.Insert(4, "");
                }
                else
                {
                    // Add to existing buildscript
                    for (int i = 0; i < rootLines.Count; i++)
                    {
                        if (rootLines[i].Contains("dependencies") && rootLines[i].Contains("{"))
                        {
                            rootLines.Insert(i + 1, "        classpath 'com.google.gms:google-services:4.4.0'");
                            break;
                        }
                    }
                }
                File.WriteAllText(rootGradle, string.Join("\n", rootLines) + "\n");
            }
        }

        // Apply plugin to launcher
        var launcherGradle = Path.Combine(path, "../launcher/build.gradle");
        if (File.Exists(launcherGradle))
        {
            var launcherLines = File.ReadAllLines(launcherGradle).ToList();
            if (!launcherLines.Any(l => l.Contains("google-services")))
            {
                launcherLines.Add("");
                launcherLines.Add("apply plugin: 'com.google.gms.google-services'");
                File.WriteAllText(launcherGradle, string.Join("\n", launcherLines) + "\n");
            }
        }
    }

    public int callbackOrder => 0;
}
#endif


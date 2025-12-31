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

        // Add classpath to root build.gradle - MUST be inside buildscript { dependencies { } }
        var rootGradle = Path.Combine(path, "../build.gradle");
        if (File.Exists(rootGradle))
        {
            var content = File.ReadAllText(rootGradle);
            
            // Check if google-services classpath is already present
            if (!content.Contains("google-services"))
            {
                // Check if buildscript block exists at the ROOT level (not inside allprojects)
                if (content.Contains("buildscript {") && !content.StartsWith("allprojects"))
                {
                    // Find buildscript dependencies block and add classpath there
                    var lines = content.Split('\n').ToList();
                    bool inBuildscript = false;
                    bool foundDependencies = false;
                    
                    for (int i = 0; i < lines.Count; i++)
                    {
                        var line = lines[i].Trim();
                        
                        // Track if we're inside the buildscript block
                        if (line.StartsWith("buildscript") && line.Contains("{"))
                        {
                            inBuildscript = true;
                        }
                        
                        // Find dependencies inside buildscript
                        if (inBuildscript && line.Contains("dependencies") && line.Contains("{"))
                        {
                            // Insert classpath after this line
                            lines.Insert(i + 1, "        classpath 'com.google.gms:google-services:4.4.0'");
                            foundDependencies = true;
                            break;
                        }
                        
                        // Exit buildscript block
                        if (inBuildscript && line == "}" && !line.Contains("{"))
                        {
                            inBuildscript = false;
                        }
                    }
                    
                    if (foundDependencies)
                    {
                        File.WriteAllText(rootGradle, string.Join("\n", lines));
                        UnityEngine.Debug.Log("[FixFirebaseGoogleServices] Added google-services classpath to buildscript.dependencies");
                    }
                }
                else
                {
                    // No buildscript block at root level - create one at the beginning
                    var newContent = "buildscript {\n" +
                                     "    repositories {\n" +
                                     "        google()\n" +
                                     "        mavenCentral()\n" +
                                     "    }\n" +
                                     "    dependencies {\n" +
                                     "        classpath 'com.google.gms:google-services:4.4.0'\n" +
                                     "    }\n" +
                                     "}\n\n" + content;
                    File.WriteAllText(rootGradle, newContent);
                    UnityEngine.Debug.Log("[FixFirebaseGoogleServices] Created buildscript block with google-services classpath");
                }
            }
            else
            {
                UnityEngine.Debug.Log("[FixFirebaseGoogleServices] google-services classpath already present");
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


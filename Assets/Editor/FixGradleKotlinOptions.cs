#if UNITY_ANDROID
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Android;

/// <summary>
/// Fixes the "Cannot invoke method kotlinOptions() on null object" error by ensuring
/// the Kotlin plugin is properly applied when kotlinOptions is used.
/// Matches the approach used by AppLovin SDK 13.5.1.
/// </summary>
public class FixGradleKotlinOptions : IPostGenerateGradleAndroidProject
{
    // Force Unity 2022.3's native Kotlin version (1.9.0) for compatibility with Firebase 20.x
    private const string KotlinVersion = "1.9.0";
    
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        try
        {
            var launcherGradlePath = Path.Combine(path, "../launcher/build.gradle");
            
            if (!File.Exists(launcherGradlePath))
            {
                UnityEngine.Debug.Log("[FixGradleKotlinOptions] launcher/build.gradle not found, skipping");
                return;
            }

            var content = File.ReadAllText(launcherGradlePath);
            var hasKotlinOptions = content.Contains("kotlinOptions");
            var hasKotlinPlugin = content.Contains("apply plugin: 'kotlin-android'") || 
                                  content.Contains("id 'org.jetbrains.kotlin.android'") ||
                                  content.Contains("kotlin(\"android\")");
            var hasBuildscriptKotlin = content.Contains("kotlin-gradle-plugin");

            UnityEngine.Debug.Log($"[FixGradleKotlinOptions] hasKotlinOptions={hasKotlinOptions}, hasKotlinPlugin={hasKotlinPlugin}, hasBuildscriptKotlin={hasBuildscriptKotlin}");

            // If kotlinOptions is present but Kotlin plugin is not applied, add Kotlin support
            if (hasKotlinOptions && !hasKotlinPlugin)
            {
                UnityEngine.Debug.Log("[FixGradleKotlinOptions] Adding Kotlin plugin support");
                content = AddKotlinSupport(content, hasBuildscriptKotlin);
                
                File.WriteAllText(launcherGradlePath, content);
                UnityEngine.Debug.Log("[FixGradleKotlinOptions] Successfully added Kotlin plugin support");
            }
            else if (hasKotlinOptions && hasKotlinPlugin)
            {
                UnityEngine.Debug.Log("[FixGradleKotlinOptions] Kotlin already configured - OK");
            }
            else
            {
                UnityEngine.Debug.Log("[FixGradleKotlinOptions] No kotlinOptions found - OK");
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"[FixGradleKotlinOptions] Exception: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private string AddKotlinSupport(string content, bool hasBuildscriptKotlin)
    {
        // Add buildscript block with Kotlin plugin if not present
        if (!hasBuildscriptKotlin)
        {
            var buildscriptBlock = $@"buildscript {{
    repositories {{
        google()
        mavenCentral()
    }}
    dependencies {{
        classpath 'org.jetbrains.kotlin:kotlin-gradle-plugin:{KotlinVersion}'
    }}
}}

";
            content = buildscriptBlock + content;
            UnityEngine.Debug.Log("[FixGradleKotlinOptions] Added buildscript block with Kotlin plugin");
        }

        // Add kotlin-android plugin after com.android.application plugin
        if (content.Contains("apply plugin: 'com.android.application'") && !content.Contains("apply plugin: 'kotlin-android'"))
        {
            content = content.Replace(
                "apply plugin: 'com.android.application'",
                "apply plugin: 'com.android.application'\napply plugin: 'kotlin-android'"
            );
            UnityEngine.Debug.Log("[FixGradleKotlinOptions] Added kotlin-android plugin");
        }

        // Add kotlin-stdlib dependency if not present
        if (!content.Contains("kotlin-stdlib"))
        {
            // Find first dependency and add kotlin-stdlib after it
            var dependenciesMatch = Regex.Match(content, @"dependencies\s*\{[^\}]*implementation");
            if (dependenciesMatch.Success)
            {
                // Find the end of the first implementation line
                var afterFirstImpl = content.IndexOf('\n', dependenciesMatch.Index + dependenciesMatch.Length);
                if (afterFirstImpl > 0)
                {
                    var kotlinStdlib = $"\n    implementation 'org.jetbrains.kotlin:kotlin-stdlib:{KotlinVersion}'";
                    content = content.Insert(afterFirstImpl, kotlinStdlib);
                    UnityEngine.Debug.Log("[FixGradleKotlinOptions] Added kotlin-stdlib dependency");
                }
            }
        }

        return content;
    }

    public int callbackOrder
    {
        // Run after EDM4U (which uses int.MaxValue - 10) but before final build
        get { return int.MaxValue - 5; }
    }
}
#endif

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

/// <summary>
/// Post-process hook for Android Gradle project generation.
/// Runs after Unity generates Gradle files but before Gradle build starts.
/// 
/// Unity 2022.3 Strategy:
/// - Let Unity auto-generate templates (don't use custom templates)
/// - Post-modify generated files for SDK compatibility
/// - Pin Kotlin to 1.9.22 for AppLovin 13.x + Firebase 21.x compatibility
/// - Keep R8 ENABLED (use packagingOptions exclusions instead of disabling)
/// </summary>
public class PostGenerateGradleAndroidProject : IPostGenerateGradleAndroidProject
{
    // Kotlin version that works with AppLovin 13.x + Firebase 21.x + Unity 2022.3
    private const string KOTLIN_VERSION = "1.9.22";
    
    public int callbackOrder => 999; // Run last after other processors
    
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log($"[PostGenerateGradle] Processing Gradle project at: {path}");
        
        // The path passed is the launcher folder, get root
        string rootPath = Directory.GetParent(path).FullName;
        
        // Priority 1: Kotlin version coordination
        EnsureKotlinVersion(rootPath);
        
        // Priority 2: Fix AppLovin dependency conflicts
        FixAppLovinDependencies(rootPath);
        
        // Priority 3: Fix streaming assets for Unity 2022.3
        FixStreamingAssets(rootPath);
        
        // Priority 4: Add R8 packaging exclusions (instead of disabling R8)
        AddPackagingExclusions(rootPath);
        
        // Priority 5: Fix immersive mode for ads
        FixImmersiveMode(rootPath);
        
        Debug.Log("[PostGenerateGradle] All post-generation fixes applied");
    }
    
    /// <summary>
    /// Pin Kotlin version to 1.9.22 for AppLovin 13.x compatibility.
    /// Unity 2022.3 needs this specific version - don't downgrade to 1.8.x!
    /// </summary>
    private void EnsureKotlinVersion(string rootPath)
    {
        string rootBuildGradle = Path.Combine(rootPath, "build.gradle");
        
        if (!File.Exists(rootBuildGradle))
        {
            Debug.LogWarning($"[PostGenerateGradle] Root build.gradle not found at: {rootBuildGradle}");
            return;
        }
        
        string content = File.ReadAllText(rootBuildGradle);
        bool modified = false;
        
        // Add ext.kotlin_version if not present
        if (!content.Contains("ext.kotlin_version"))
        {
            if (content.Contains("buildscript {"))
            {
                content = content.Replace(
                    "buildscript {",
                    $"buildscript {{\n    ext.kotlin_version = '{KOTLIN_VERSION}'"
                );
                modified = true;
                Debug.Log($"[PostGenerateGradle] Added ext.kotlin_version = '{KOTLIN_VERSION}'");
            }
        }
        
        // Add Kotlin Gradle plugin classpath if not present
        if (!content.Contains("kotlin-gradle-plugin") && content.Contains("dependencies {"))
        {
            // Find the buildscript dependencies block
            var regex = new Regex(@"(buildscript\s*\{[^}]*dependencies\s*\{)");
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, 
                    $"$1\n        classpath \"org.jetbrains.kotlin:kotlin-gradle-plugin:$kotlin_version\"");
                modified = true;
                Debug.Log("[PostGenerateGradle] Added Kotlin Gradle plugin classpath");
            }
        }
        
        if (modified)
        {
            File.WriteAllText(rootBuildGradle, content);
        }
    }
    
    /// <summary>
    /// Add explicit Kotlin stdlib dependency to unityLibrary for AppLovin 13.x.
    /// This prevents version conflicts between Firebase and AppLovin.
    /// </summary>
    private void FixAppLovinDependencies(string rootPath)
    {
        string unityLibraryBuildGradle = Path.Combine(rootPath, "unityLibrary", "build.gradle");
        
        if (!File.Exists(unityLibraryBuildGradle))
        {
            Debug.LogWarning($"[PostGenerateGradle] unityLibrary build.gradle not found");
            return;
        }
        
        string content = File.ReadAllText(unityLibraryBuildGradle);
        bool modified = false;
        
        // Add Kotlin stdlib with explicit version if not present
        if (!content.Contains("kotlin-stdlib"))
        {
            // Find the dependencies block and add Kotlin stdlib
            var regex = new Regex(@"(dependencies\s*\{)");
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, 
                    $"$1\n    // Kotlin stdlib pinned for AppLovin 13.x + Firebase 21.x compatibility\n    implementation \"org.jetbrains.kotlin:kotlin-stdlib:{KOTLIN_VERSION}\"",
                    1); // Only first match
                modified = true;
                Debug.Log($"[PostGenerateGradle] Added Kotlin stdlib {KOTLIN_VERSION} to unityLibrary");
            }
        }
        
        if (modified)
        {
            File.WriteAllText(unityLibraryBuildGradle, content);
        }
    }
    
    /// <summary>
    /// Fix streaming assets configuration for Unity 2022.3.
    /// The format changed from **STREAMING_ASSETS** placeholder to explicit list.
    /// </summary>
    private void FixStreamingAssets(string rootPath)
    {
        string launcherBuildGradle = Path.Combine(rootPath, "launcher", "build.gradle");
        
        if (!File.Exists(launcherBuildGradle))
        {
            Debug.LogWarning($"[PostGenerateGradle] Launcher build.gradle not found");
            return;
        }
        
        string content = File.ReadAllText(launcherBuildGradle);
        
        // Check if aaptOptions already has proper noCompress
        if (content.Contains("aaptOptions") && content.Contains("noCompress"))
        {
            Debug.Log("[PostGenerateGradle] aaptOptions already configured in launcher");
            return;
        }
        
        // Unity 2022.3 should handle this automatically, but verify it exists
        // If not, we'd add it here. For now, just log status.
        if (!content.Contains("aaptOptions"))
        {
            Debug.Log("[PostGenerateGradle] No aaptOptions in launcher (Unity 2022.3 handles this via unityLibrary)");
        }
    }
    
    /// <summary>
    /// Add R8 packaging exclusions instead of disabling R8.
    /// This is the Unity 2022.3 best practice.
    /// </summary>
    private void AddPackagingExclusions(string rootPath)
    {
        string unityLibraryBuildGradle = Path.Combine(rootPath, "unityLibrary", "build.gradle");
        
        if (!File.Exists(unityLibraryBuildGradle))
        {
            return;
        }
        
        string content = File.ReadAllText(unityLibraryBuildGradle);
        
        // Check if packaging exclusions already exist
        if (content.Contains("kotlin-tooling-metadata.json"))
        {
            Debug.Log("[PostGenerateGradle] R8 packaging exclusions already present");
            return;
        }
        
        // Find packagingOptions block or android block to add exclusions
        string exclusions = @"
    packagingOptions {
        exclude 'META-INF/kotlin-tooling-metadata.json'
        exclude 'META-INF/INDEX.LIST'
        exclude 'META-INF/io.netty.versions.properties'
        exclude 'META-INF/DEPENDENCIES'
    }";
        
        // Try to add after existing packagingOptions or before closing android block
        if (content.Contains("packagingOptions {"))
        {
            // Add to existing packagingOptions
            content = content.Replace(
                "packagingOptions {",
                @"packagingOptions {
        exclude 'META-INF/kotlin-tooling-metadata.json'
        exclude 'META-INF/INDEX.LIST'
        exclude 'META-INF/io.netty.versions.properties'
        exclude 'META-INF/DEPENDENCIES'"
            );
            File.WriteAllText(unityLibraryBuildGradle, content);
            Debug.Log("[PostGenerateGradle] Added R8 packaging exclusions to existing block");
        }
        else if (content.Contains("android {"))
        {
            // Find last closing brace of android block and add before it
            // This is more complex, so we'll add after the android { line
            var regex = new Regex(@"(android\s*\{)");
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, $"$1{exclusions}", 1);
                File.WriteAllText(unityLibraryBuildGradle, content);
                Debug.Log("[PostGenerateGradle] Added R8 packaging exclusions to android block");
            }
        }
    }
    
    /// <summary>
    /// Adds immersive mode to the Unity library manifest.
    /// This ensures ads don't cause the status bar to appear.
    /// </summary>
    private void FixImmersiveMode(string rootPath)
    {
        string unityLibraryManifest = Path.Combine(rootPath, "unityLibrary", "src", "main", "AndroidManifest.xml");
        
        if (!File.Exists(unityLibraryManifest))
        {
            Debug.LogWarning($"[PostGenerateGradle] Unity library manifest not found");
            return;
        }
        
        string content = File.ReadAllText(unityLibraryManifest);
        
        if (!content.Contains("android:immersive=\"true\""))
        {
            content = content.Replace(
                "<application",
                "<application\n        android:immersive=\"true\""
            );
            File.WriteAllText(unityLibraryManifest, content);
            Debug.Log("[PostGenerateGradle] Added immersive mode to Unity library manifest");
        }
    }
}

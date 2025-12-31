using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.Android;
using UnityEngine;

/// <summary>
/// Simplified post-process hook for Android Gradle project generation.
/// 
/// CLEANUP (Dec 11, 2025): Removed redundant methods that duplicated EDM4U/AppLovin functionality:
/// - EnsureKotlinVersion() - Handled by EDM4U and AppLovin post-processors
/// - FixAppLovinDependencies() - Duplicate of above
/// - FixStreamingAssets() - Did nothing, just logged
/// - FixImmersiveMode() - REMOVED: Was blocking ad close button overlays
/// 
/// Only kept: AddPackagingExclusions() for R8 META-INF conflict resolution
/// </summary>
public class PostGenerateGradleAndroidProject : IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 999; // Run last after other processors
    
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log($"[PostGenerateGradle] Processing Gradle project at: {path}");
        
        // The path passed is the launcher folder, get root
        string rootPath = Directory.GetParent(path).FullName;
        
        // Only action: Add R8 packaging exclusions (instead of disabling R8)
        AddPackagingExclusions(rootPath);
        
        Debug.Log("[PostGenerateGradle] Post-generation fixes applied");
    }
    
    /// <summary>
    /// Add R8 packaging exclusions instead of disabling R8.
    /// This is the Unity 2022.3 best practice for handling META-INF conflicts.
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
            // Add after the android { line
            var regex = new Regex(@"(android\s*\{)");
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, $"$1{exclusions}", 1);
                File.WriteAllText(unityLibraryBuildGradle, content);
                Debug.Log("[PostGenerateGradle] Added R8 packaging exclusions to android block");
            }
        }
    }
}
